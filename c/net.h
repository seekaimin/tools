#ifndef __NET_H__
#define __NET_H__

typedef char NET_ENDIAN;

#define	NET_BIG_ENDIAN	    1   
#define	NET_SMALL_ENDIAN	0


typedef struct _NET_INDEX
{
    unsigned int index;
    NET_ENDIAN endian;   
}NET_INDEX;



unsigned char NET_GetChar(unsigned char* buffer, NET_INDEX* ni);
unsigned short NET_GetShort(unsigned char* buffer, NET_INDEX* ni);
unsigned int NET_GetInt(unsigned char* buffer, NET_INDEX* ni);
unsigned long NET_GetLong(unsigned char* buffer, NET_INDEX* ni);

void NET_CopyChar(unsigned char* buffer, unsigned char value, NET_INDEX* ni);
void NET_CopyShort(unsigned char* buffer, unsigned short value, NET_INDEX* ni);
void NET_CopyInt(unsigned char* buffer, unsigned int value, NET_INDEX* ni);
void NET_CopyLong(unsigned char* buffer, unsigned long value, NET_INDEX* ni);
#endif // COMMON

