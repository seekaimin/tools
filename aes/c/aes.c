#include "aes.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
/*
 * 32-bit integer manipulation macros (little endian)
 */
#define GET_ULONG_LE(n,b,i)                             \
{                                                       \
    (n) = ( (unsigned int) (b)[(i)    ]       )        \
        | ( (unsigned int) (b)[(i) + 1] <<  8 )        \
        | ( (unsigned int) (b)[(i) + 2] << 16 )        \
        | ( (unsigned int) (b)[(i) + 3] << 24 );       \
}

#define PUT_ULONG_LE(n,b,i)                             \
{                                                       \
    (b)[(i)    ] = (unsigned char) ( (n)       );       \
    (b)[(i) + 1] = (unsigned char) ( (n) >>  8 );       \
    (b)[(i) + 2] = (unsigned char) ( (n) >> 16 );       \
    (b)[(i) + 3] = (unsigned char) ( (n) >> 24 );       \
}
/*
 * Tables generation code
 */
#define ROTL8(x) ( ( x << 8 ) & 0xFFFFFFFF ) | ( x >> 24 )
#define XTIME(x) ( ( x << 1 ) ^ ( ( x & 0x80 ) ? 0x1B : 0x00 ) )
#define MUL(x,y) ( ( x && y ) ? pow[(log[x]+log[y]) % 255] : 0 )

#define AES_FROUND(X0,X1,X2,X3,Y0,Y1,Y2,Y3)     \
{                                               \
	X0 = *RK++ ^ FT0[ ( Y0       ) & 0xFF ] ^   \
		FT1[ ( Y1 >>  8 ) & 0xFF ] ^   \
		FT2[ ( Y2 >> 16 ) & 0xFF ] ^   \
		FT3[ ( Y3 >> 24 ) & 0xFF ];    \
	                        \
	X1 = *RK++ ^ FT0[ ( Y1       ) & 0xFF ] ^   \
		FT1[ ( Y2 >>  8 ) & 0xFF ] ^   \
		FT2[ ( Y3 >> 16 ) & 0xFF ] ^   \
		FT3[ ( Y0 >> 24 ) & 0xFF ];    \
	                        \
	X2 = *RK++ ^ FT0[ ( Y2       ) & 0xFF ] ^   \
		FT1[ ( Y3 >>  8 ) & 0xFF ] ^   \
		FT2[ ( Y0 >> 16 ) & 0xFF ] ^   \
		FT3[ ( Y1 >> 24 ) & 0xFF ];    \
	                        \
	X3 = *RK++ ^ FT0[ ( Y3       ) & 0xFF ] ^   \
		FT1[ ( Y0 >>  8 ) & 0xFF ] ^   \
		FT2[ ( Y1 >> 16 ) & 0xFF ] ^   \
		FT3[ ( Y2 >> 24 ) & 0xFF ];    \
}

#define AES_RROUND(X0,X1,X2,X3,Y0,Y1,Y2,Y3)     \
{                                               \
	X0 = *RK++ ^ RT0[ ( Y0       ) & 0xFF ] ^   \
		RT1[ ( Y3 >>  8 ) & 0xFF ] ^   \
		RT2[ ( Y2 >> 16 ) & 0xFF ] ^   \
		RT3[ ( Y1 >> 24 ) & 0xFF ];    \
	                        \
	X1 = *RK++ ^ RT0[ ( Y1       ) & 0xFF ] ^   \
		RT1[ ( Y0 >>  8 ) & 0xFF ] ^   \
		RT2[ ( Y3 >> 16 ) & 0xFF ] ^   \
		RT3[ ( Y2 >> 24 ) & 0xFF ];    \
	                        \
	X2 = *RK++ ^ RT0[ ( Y2       ) & 0xFF ] ^   \
		RT1[ ( Y1 >>  8 ) & 0xFF ] ^   \
		RT2[ ( Y0 >> 16 ) & 0xFF ] ^   \
		RT3[ ( Y3 >> 24 ) & 0xFF ];    \
	                        \
	X3 = *RK++ ^ RT0[ ( Y3       ) & 0xFF ] ^   \
		RT1[ ( Y2 >>  8 ) & 0xFF ] ^   \
		RT2[ ( Y1 >> 16 ) & 0xFF ] ^   \
		RT3[ ( Y0 >> 24 ) & 0xFF ];    \
}

/*
 * Forward S-box & tables
 */
static unsigned char FSb[256];
static unsigned int FT0[256];
static unsigned int FT1[256];
static unsigned int FT2[256];
static unsigned int FT3[256];

/*
 * Reverse S-box & tables
 */
static unsigned char RSb[256];
static unsigned int RT0[256];
static unsigned int RT1[256];
static unsigned int RT2[256];
static unsigned int RT3[256];

/*
 * Round constants
 */
static unsigned int RCON[10];

static void AES_Gen_Tables()
{
    int i, x, y, z;
    int pow[256];
    int log[256];

    /*
    * compute pow and log tables over GF(2^8)
    */
    for(i = 0, x = 1; i < 256; i++)
    {
        pow[i] = x;
        log[x] = i;
        x = (x ^ XTIME(x)) & 0xFF;
    }

    /*
    * calculate the round constants
    */
    for(i = 0, x = 1; i < 10; i++)
    {
        RCON[i] = (unsigned int)x;
        x = XTIME(x) & 0xFF;
    }

    /*
    * generate the forward and reverse S-boxes
    */
    FSb[0x00] = 0x63;
    RSb[0x63] = 0x00;

    for(i = 1; i < 256; i++)
    {
        x = pow[255 - log[i]];

        y  = x;
        y = ((y << 1) | (y >> 7)) & 0xFF;
        x ^= y;
        y = ((y << 1) | (y >> 7)) & 0xFF;
        x ^= y;
        y = ((y << 1) | (y >> 7)) & 0xFF;
        x ^= y;
        y = ((y << 1) | (y >> 7)) & 0xFF;
        x ^= y ^ 0x63;

        FSb[i] = (unsigned char)x;
        RSb[x] = (unsigned char)i;
    }

    /*
    * generate the forward and reverse tables
    */
    for(i = 0; i < 256; i++)
    {
        x = FSb[i];
        y = XTIME( x ) & 0xFF;
        z =  ( y ^ x ) & 0xFF;

        FT0[i] = ((unsigned int)y      ) ^
                 ((unsigned int)x <<  8) ^
                 ((unsigned int)x << 16) ^
                 ((unsigned int)z << 24);

        FT1[i] = ROTL8(FT0[i]);
        FT2[i] = ROTL8(FT1[i]);
        FT3[i] = ROTL8(FT2[i]);

        x = RSb[i];

        RT0[i] = ((unsigned int)MUL( 0x0E, x )       ) ^
                 ((unsigned int)MUL( 0x09, x ) <<  8 ) ^
                 ((unsigned int)MUL( 0x0D, x ) << 16 ) ^
                 ((unsigned int)MUL( 0x0B, x ) << 24 );

        RT1[i] = ROTL8(RT0[i]);
        RT2[i] = ROTL8(RT1[i]);
        RT3[i] = ROTL8(RT2[i]);
    }
}

/*
 * AES key schedule (encryption)
 */
void AES_Setkey_Enc(ST_CA_AES_CONTEXT *ctx, unsigned char *key, int keysize)
{
    int i;
    unsigned int *RK;

    AES_Gen_Tables();
    switch(keysize)
    {
        case 128:
        {
            ctx->nr = 10;
        }
        break;
        case 192:
        {
            ctx->nr = 12;
        }
        break;
        case 256:
        {
            ctx->nr = 14;
        }
        break;
        default :
        {
            return;
        }
    }

    ctx->rk = RK = ctx->buf;

    for(i = 0; i < (keysize >> 5); i++)
    {
        GET_ULONG_LE(RK[i], key, i << 2);
    }

    switch(ctx->nr)
    {
        case 10:
        {
            for( i = 0; i < 10; i++, RK += 4 )
            {
                RK[4]  = RK[0] ^ RCON[i] ^
                         ( (unsigned int) FSb[ ( RK[3] >>  8 ) & 0xFF ]       ) ^
                         ( (unsigned int) FSb[ ( RK[3] >> 16 ) & 0xFF ] <<  8 ) ^
                         ( (unsigned int) FSb[ ( RK[3] >> 24 ) & 0xFF ] << 16 ) ^
                         ( (unsigned int) FSb[ ( RK[3]       ) & 0xFF ] << 24 );

                RK[5]  = RK[1] ^ RK[4];
                RK[6]  = RK[2] ^ RK[5];
                RK[7]  = RK[3] ^ RK[6];
            }
        }
        break;
        case 12:
        {
            for( i = 0; i < 8; i++, RK += 6 )
            {
                RK[6]  = RK[0] ^ RCON[i] ^
                         ( (unsigned int) FSb[ ( RK[5] >>  8 ) & 0xFF ]       ) ^
                         ( (unsigned int) FSb[ ( RK[5] >> 16 ) & 0xFF ] <<  8 ) ^
                         ( (unsigned int) FSb[ ( RK[5] >> 24 ) & 0xFF ] << 16 ) ^
                         ( (unsigned int) FSb[ ( RK[5]       ) & 0xFF ] << 24 );

                RK[7]  = RK[1] ^ RK[6];
                RK[8]  = RK[2] ^ RK[7];
                RK[9]  = RK[3] ^ RK[8];
                RK[10] = RK[4] ^ RK[9];
                RK[11] = RK[5] ^ RK[10];
            }
        }
        break;
        case 14:
        {
            for( i = 0; i < 7; i++, RK += 8 )
            {
                RK[8]  = RK[0] ^ RCON[i] ^
                         ( (unsigned int) FSb[ ( RK[7] >>  8 ) & 0xFF ]       ) ^
                         ( (unsigned int) FSb[ ( RK[7] >> 16 ) & 0xFF ] <<  8 ) ^
                         ( (unsigned int) FSb[ ( RK[7] >> 24 ) & 0xFF ] << 16 ) ^
                         ( (unsigned int) FSb[ ( RK[7]       ) & 0xFF ] << 24 );

                RK[9]  = RK[1] ^ RK[8];
                RK[10] = RK[2] ^ RK[9];
                RK[11] = RK[3] ^ RK[10];

                RK[12] = RK[4] ^
                         ( (unsigned int) FSb[ ( RK[11]       ) & 0xFF ]       ) ^
                         ( (unsigned int) FSb[ ( RK[11] >>  8 ) & 0xFF ] <<  8 ) ^
                         ( (unsigned int) FSb[ ( RK[11] >> 16 ) & 0xFF ] << 16 ) ^
                         ( (unsigned int) FSb[ ( RK[11] >> 24 ) & 0xFF ] << 24 );

                RK[13] = RK[5] ^ RK[12];
                RK[14] = RK[6] ^ RK[13];
                RK[15] = RK[7] ^ RK[14];
            }
        }
        break;
        default:
        {
            break;
        }
    }
}

/*
 * AES key schedule (decryption)
 */
void AES_Setkey_Dec(ST_CA_AES_CONTEXT *ctx, unsigned char *key, int keysize)
{
    int i, j;
    ST_CA_AES_CONTEXT cty;
    unsigned int *RK;
    unsigned int *SK;

    switch( keysize )
    {
        case 128:
        {
            ctx->nr = 10;
        }
        break;
        case 192:
        {
            ctx->nr = 12;
        }
        break;
        case 256:
        {
            ctx->nr = 14;
        }
        break;
        default :
        {
            return;
        }
    }

    ctx->rk = RK = ctx->buf;

    AES_Setkey_Enc(&cty, key, keysize);
    SK = cty.rk + cty.nr * 4;

    *RK++ = *SK++;
    *RK++ = *SK++;
    *RK++ = *SK++;
    *RK++ = *SK++;

    for(i = ctx->nr - 1, SK -= 8; i > 0; i--, SK -= 8)
    {
        for(j = 0; j < 4; j++, SK++)
        {
            *RK++ = RT0[ FSb[ ( *SK       ) & 0xFF ] ] ^
                    RT1[ FSb[ ( *SK >>  8 ) & 0xFF ] ] ^
                    RT2[ FSb[ ( *SK >> 16 ) & 0xFF ] ] ^
                    RT3[ FSb[ ( *SK >> 24 ) & 0xFF ] ];
        }
    }

    *RK++ = *SK++;
    *RK++ = *SK++;
    *RK++ = *SK++;
    *RK++ = *SK++;

    memset(&cty, 0, sizeof(ST_CA_AES_CONTEXT));
}

/*
 * AES-ECB block encryption/decryption
 */
void AES_Crypt_Ecb(ST_CA_AES_CONTEXT *ctx, int mode, unsigned char* input, unsigned char* output)
{
    int i;
    unsigned int *RK, X0, X1, X2, X3, Y0, Y1, Y2, Y3;

    RK = ctx->rk;

    GET_ULONG_LE(X0, input, 0);
    X0 ^= *RK++;
    GET_ULONG_LE(X1, input, 4);
    X1 ^= *RK++;
    GET_ULONG_LE(X2, input, 8);
    X2 ^= *RK++;
    GET_ULONG_LE(X3, input, 12);
    X3 ^= *RK++;

    if(mode == CRYPT_TYPE_DECRYPT)
    {
        for(i = (ctx->nr >> 1) - 1; i > 0; i--)
        {
            AES_RROUND(Y0, Y1, Y2, Y3, X0, X1, X2, X3);
            AES_RROUND(X0, X1, X2, X3, Y0, Y1, Y2, Y3);
        }

        AES_RROUND(Y0, Y1, Y2, Y3, X0, X1, X2, X3);

        X0 = *RK++ ^ \
             ( (unsigned int) RSb[ ( Y0       ) & 0xFF ]       ) ^
             ( (unsigned int) RSb[ ( Y3 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) RSb[ ( Y2 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) RSb[ ( Y1 >> 24 ) & 0xFF ] << 24 );

        X1 = *RK++ ^ \
             ( (unsigned int) RSb[ ( Y1       ) & 0xFF ]       ) ^
             ( (unsigned int) RSb[ ( Y0 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) RSb[ ( Y3 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) RSb[ ( Y2 >> 24 ) & 0xFF ] << 24 );

        X2 = *RK++ ^ \
             ( (unsigned int) RSb[ ( Y2       ) & 0xFF ]       ) ^
             ( (unsigned int) RSb[ ( Y1 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) RSb[ ( Y0 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) RSb[ ( Y3 >> 24 ) & 0xFF ] << 24 );

        X3 = *RK++ ^ \
             ( (unsigned int) RSb[ ( Y3       ) & 0xFF ]       ) ^
             ( (unsigned int) RSb[ ( Y2 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) RSb[ ( Y1 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) RSb[ ( Y0 >> 24 ) & 0xFF ] << 24 );
    }
    else /* AS_CRYPT_TYPE_DECRYPT */
    {
        for( i = (ctx->nr >> 1) - 1; i > 0; i-- )
        {
            AES_FROUND(Y0, Y1, Y2, Y3, X0, X1, X2, X3);
            AES_FROUND(X0, X1, X2, X3, Y0, Y1, Y2, Y3);
        }

        AES_FROUND(Y0, Y1, Y2, Y3, X0, X1, X2, X3);

        X0 = *RK++ ^ \
             ( (unsigned int) FSb[ ( Y0       ) & 0xFF ]       ) ^
             ( (unsigned int) FSb[ ( Y1 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) FSb[ ( Y2 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) FSb[ ( Y3 >> 24 ) & 0xFF ] << 24 );

        X1 = *RK++ ^ \
             ( (unsigned int) FSb[ ( Y1       ) & 0xFF ]       ) ^
             ( (unsigned int) FSb[ ( Y2 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) FSb[ ( Y3 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) FSb[ ( Y0 >> 24 ) & 0xFF ] << 24 );

        X2 = *RK++ ^ \
             ( (unsigned int) FSb[ ( Y2       ) & 0xFF ]       ) ^
             ( (unsigned int) FSb[ ( Y3 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) FSb[ ( Y0 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) FSb[ ( Y1 >> 24 ) & 0xFF ] << 24 );

        X3 = *RK++ ^ \
             ( (unsigned int) FSb[ ( Y3       ) & 0xFF ]       ) ^
             ( (unsigned int) FSb[ ( Y0 >>  8 ) & 0xFF ] <<  8 ) ^
             ( (unsigned int) FSb[ ( Y1 >> 16 ) & 0xFF ] << 16 ) ^
             ( (unsigned int) FSb[ ( Y2 >> 24 ) & 0xFF ] << 24 );
    }

    PUT_ULONG_LE(X0, output, 0);
    PUT_ULONG_LE(X1, output, 4);
    PUT_ULONG_LE(X2, output, 8);
    PUT_ULONG_LE(X3, output, 12);
}

/*
 * AES-CBC buffer encryption/decryption
 */
void AES_Crypt_Cbc(ST_CA_AES_CONTEXT *ctx, int mode,  int length,
                         unsigned char* iv, unsigned char *input, unsigned char *output)
{
    int i;
    unsigned char temp[16];

    if(mode == CRYPT_TYPE_DECRYPT)
    {
        while( length > 0 )
        {
            memcpy(temp, input, 16);
            AES_Crypt_Ecb(ctx, mode, input, output);

            for( i = 0; i < 16; i++ )
            {
                output[i] = (unsigned char)(output[i] ^ iv[i]);
            }

            memcpy(iv, temp, 16);

            input  += 16;
            output += 16;
            length -= 16;
        }
    }
    else
    {
        while(length > 0)
        {
            for(i = 0; i < 16; i++)
            {
                output[i] = (unsigned char)(input[i] ^ iv[i]);
            }

            AES_Crypt_Ecb(ctx, mode, output, output);
            memcpy(iv, output, 16);

            input  += 16;
            output += 16;
            length -= 16;
        }
    }
}

/*
 * AES-CFB128 buffer encryption/decryption
 */
void AES_Crypt_Cfb128(ST_CA_AES_CONTEXT *ctx, int mode, int length, int *iv_off, unsigned char iv[16], unsigned char *input,  unsigned char *output)
{
    int c, n = *iv_off;

    if(mode == CRYPT_TYPE_DECRYPT)
    {
        while( length-- )
        {
            if(n == 0)
            {
                AES_Crypt_Ecb( ctx, CRYPT_TYPE_DECRYPT, iv, iv );
            }

            c = *input++;
            *output++ = (unsigned char)( c ^ iv[n] );
            iv[n] = (unsigned char) c;

            n = (n + 1) & 0x0F;
        }
    }
    else
    {
        while(length--)
        {
            if(n == 0)
            {
                AES_Crypt_Ecb(ctx, CRYPT_TYPE_DECRYPT, iv, iv);
            }

            iv[n] = *output++ = (unsigned char)(iv[n] ^ *input++);

            n = (n + 1) & 0x0F;
        }
    }

    *iv_off = n;
}

void AES_Print(unsigned char* buffer,int len){
	printf("\n ################# START ################## \n");
	for (int i = 0; i < len; i++) {
		printf("%02X ", buffer[i]);
	}
	printf("\n ################# END ################## \n");
}
	

BOOL AES_EncBlock(unsigned char* des, unsigned char* src, unsigned int offset, unsigned int srcLen,unsigned char def)
{
	memset(des,def,16);
	if(offset > srcLen){
		return FALSE;
	}
	else if(offset == srcLen) 
	{
		
	}
	if(srcLen < offset + 16)
	{
		memcpy(des,src + offset,srcLen - offset);
	}
	else
	{
		memcpy(des,src + offset,16);	
	}
	return TRUE;
}

BOOL AES_DesBlock(unsigned char* des, unsigned char* src, unsigned int offset, unsigned int srcLen,unsigned char def)
{
	memset(des,def,16);
	if(offset >= srcLen){
		return FALSE;
	}
	else if(srcLen < offset + 16)
	{
		memcpy(des,src + offset,srcLen - offset);
	}
	else
	{
		memcpy(des,src + offset,16);	
	}
	return TRUE;
}

BOOL AES_Encrypt(EN_CRYPT_MODE enMode, PADDING_MODE padMode, unsigned char* input, unsigned char* output, unsigned int inputLen, unsigned int* outputLen, unsigned char* Key,  unsigned char keyLen)
{
    if(input == NULL || output == NULL || Key == NULL)
    {
        return FALSE;
    }
    if(keyLen != 8 && keyLen != 16 && keyLen != 24)
    {
        return FALSE;
    }
	unsigned char inBlock[16] = {0};
	if(inBlock == NULL){
		return FALSE;
	}
	unsigned char outBlock[16] = {0};
	if(outBlock == NULL){
		return FALSE;
	}
    ST_CA_AES_CONTEXT ctx;
    unsigned char iv[16];
    unsigned char ucKeyLenBits = 0;
    int i = 0, j = 0;
	int maxLen = (inputLen / 16 + 1) * 16;
	unsigned char def = (maxLen - inputLen) & 0xFF;
	*outputLen = maxLen;
    ucKeyLenBits = keyLen<<3;
	AES_Setkey_Enc(&ctx, Key, ucKeyLenBits);
	unsigned int offset = 0;
    if(enMode == CRYPT_MODE_ECB) //CRYPT_MODE_ECB
    {
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_EncBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Ecb(&ctx, CRYPT_TYPE_ENCRYPT, inBlock, outBlock);
			memcpy(output + offset, outBlock,16);
		}
    }
	else if(enMode == CRYPT_MODE_CBC) //CRYPT_MODE_CBC
    {
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_EncBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Cbc(&ctx, CRYPT_TYPE_ENCRYPT, 16, iv, inBlock, outBlock );
			memcpy(output + offset, outBlock,16);
		}
    }
	else if(enMode == CRYPT_MODE_CFB128) //CRYPT_MODE_CFB128
    {
		int ivOffset;
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_EncBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Cfb128(&ctx, CRYPT_TYPE_DECRYPT, 64, &ivOffset, iv, inBlock, outBlock);
			memcpy(output + offset, outBlock,16);
		}
    }
    return TRUE;
}


BOOL AES_Decrypt(EN_CRYPT_MODE enMode, PADDING_MODE padMode, unsigned char* input, unsigned char* output, unsigned int inputLen, unsigned int* outputLen, unsigned char* Key,  unsigned char keyLen)
{
	*outputLen = 0;
    if(input == NULL || output == NULL || Key == NULL)
    {
        return FALSE;
    }
    if(keyLen != 8 && keyLen != 16 && keyLen != 24)
    {
        return FALSE;
    }
	unsigned char inBlock[16] = {0};
	if(inBlock == NULL){
		return FALSE;
	}
	unsigned char outBlock[16] = {0};
	if(outBlock == NULL){
		return FALSE;
	}
    ST_CA_AES_CONTEXT ctx;
    unsigned char iv[16];
    unsigned char ucKeyLenBits = 0;
    int i = 0, j = 0;
	int maxLen = (inputLen / 16) * 16;
	unsigned char def = (maxLen - inputLen) & 0xFF;
    ucKeyLenBits = keyLen<<3;
	AES_Setkey_Dec(&ctx, Key, ucKeyLenBits);
	unsigned int offset = 0;
    if(enMode == CRYPT_MODE_ECB) //CRYPT_MODE_ECB
    {
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_DesBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Ecb(&ctx, CRYPT_TYPE_DECRYPT, inBlock, outBlock);
			memcpy(output + offset, outBlock,16);
		}
    }
	else if(enMode == CRYPT_MODE_CBC) //CRYPT_MODE_CBC
    {
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_DesBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Cbc(&ctx, CRYPT_TYPE_DECRYPT, 16, iv, inBlock, outBlock );
			memcpy(output + offset, outBlock,16);
		}
    }
	else if(enMode == CRYPT_MODE_CFB128) //CRYPT_MODE_CFB128
    {
		int ivOffset;
		for(i = 0, j = maxLen >> 4; i < j; ++i, offset += 16)
		{
			if(FALSE == AES_DesBlock(inBlock, input, offset, inputLen, def))
			{
				break;
			}
			AES_Crypt_Cfb128(&ctx, CRYPT_TYPE_DECRYPT, 64, &ivOffset, iv, inBlock, outBlock);
			memcpy(output + offset, outBlock,16);
		}
    }
	*outputLen = maxLen - output[maxLen - 1];
    return TRUE;
}


int AES_RunAes(EN_CRYPT_TYPE enType, EN_CRYPT_MODE enMode, PADDING_MODE padMode, unsigned char* input, unsigned char* output, unsigned int inputLen, unsigned int* outputLen, unsigned char* Key,  unsigned char keyLen)
{
    BOOL result = FALSE;
	if(enType == CRYPT_TYPE_ENCRYPT)
	{
		result = AES_Encrypt(enMode, padMode, input, output, inputLen, outputLen, Key, keyLen);
	}
	else if(enType == CRYPT_TYPE_DECRYPT)
	{
		result = AES_Decrypt(enMode, padMode, input, output, inputLen, outputLen, Key, keyLen);		
	}
    return result;
}
















