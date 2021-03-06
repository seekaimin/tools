
#include <string.h>
#include <stdio.h>
#include <time.h>
#include "common.h"
#include "aes.h"

void main(){
	unsigned char keyLength = 16; //密钥
	unsigned int inLength = 10;
	unsigned char key[16] = {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F};
	unsigned char in[64] = {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F,0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F,0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F,0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F};
	unsigned char out1[128];
	unsigned char out2[128];
	unsigned int outLength = 0;
	int i;
	
	EN_CRYPT_MODE enMode = CRYPT_MODE_ECB;//CRYPT_MODE_CBC;CRYPT_MODE_ECB; //加密方式
	PADDING_MODE padMode = PADDING_PKCS7; //补齐方式
	
	//0xFC,0x48,0x01,0x06,0xC4,0x39,0xB1,0xE7,0x61,0x58,0x96,0x24,0x2A,0x9E,0x72,0x85
	//加密
	printf("\n***************** encrypt START ************************\n");
	BOOL result = AES_RunAes(CRYPT_TYPE_ENCRYPT,enMode,padMode,in, out1,inLength,&outLength,key,keyLength);
	printf("result=%d-------outLength=%d \n", result,outLength);
	if(result == TRUE)
	{
		for (i = 0; i < outLength; i++) {
			printf("%02X ", out1[i]);
		}
	}
	printf("\n***************** encrypt END ************************\n");

	printf("\n***************** decrypt START ************************\n");
	inLength = outLength;
	outLength = 0;
	//解密
	result = AES_RunAes(CRYPT_TYPE_DECRYPT,enMode,padMode,out1, out2,inLength,&outLength,key,keyLength);
	printf("result=%d-------outLength=%d \n", result,outLength);
	if(result == TRUE)
	{
		for (i = 0; i < outLength; i++) {
			printf("%02X ", out2[i]);
		}
	}
	printf("\n***************** decrypt END ************************\n");
}