#include <string.h>
#include "net.h"

unsigned long net_get(unsigned char* buffer, int len, NET_ENDIAN ne)
{
    unsigned long result = 0;
    if(ne == NET_BIG_ENDIAN)
    {
        for(int i = 0; i < len; i++)
        {
            result |= buffer[i] << ((len - i - 1) * 8);
        }
    }
    else if(ne == NET_SMALL_ENDIAN)
    {
        for(int i = len - 1; i >= 0; i--)
        {
            result |= buffer[i] << (i * 8);
        }
    }
    return result;
}

void net_copy(unsigned char* output, unsigned long value, int len, NET_ENDIAN ne)
{
    if(ne == NET_BIG_ENDIAN)
    {
        for(int i = 0; i < len; i++)
        {
            output[i] = (unsigned char)((value >> (8 * (len-1-i))) & 0xFF);
        }
    }
    else if(ne == NET_SMALL_ENDIAN)
    {
        for(int i = len - 1; i >= 0; i--)
        {
            output[i] = (unsigned char)((value >> (8 * i)) & 0xFF);
        }
    }
}

unsigned char NET_GetChar(unsigned char* buffer, NET_INDEX* ni){
    int len = 1;
    unsigned char val = (buffer + ni->index)[0];
    ni->index = ni->index + len;
    return val;
}
unsigned short NET_GetShort(unsigned char* buffer, NET_INDEX* ni){
    unsigned int len = 2;
    unsigned short val = net_get(buffer + ni->index, len, ni->endian) & 0xFFFF;
    ni->index = ni->index + len;
    return val;
}

unsigned int NET_GetInt(unsigned char* buffer, NET_INDEX* ni){
    unsigned int len = 4;
    unsigned int val = net_get(buffer + ni->index, len, ni->endian) & 0xFFFFFFFF;
    ni->index = ni->index + len;
    return val;
}
unsigned long NET_GetLong(unsigned char* buffer, NET_INDEX* ni){
    unsigned int len = 8;
    unsigned long val = net_get(buffer + ni->index, len, ni->endian) & 0xFFFFFFFFFFFFFFFF;
    ni->index = ni->index + len;
    return val;
}

void NET_CopyChar(unsigned char* buffer, unsigned char value, NET_INDEX* ni)
{
    unsigned index = ni->index;
    buffer[index] = value;
    ni->index = index + 1;
}
void NET_CopyShort(unsigned char* buffer, unsigned short value, NET_INDEX* ni)
{
    unsigned index = ni->index;
    int len = 2;
    unsigned char temp[len];
    net_copy(temp, value, len, ni->endian);
    memcpy(buffer + ni->index, temp, len);
    ni->index = index + len;
}
void NET_CopyInt(unsigned char* buffer, unsigned int value, NET_INDEX* ni)
{
    unsigned index = ni->index;
    int len = 4;
    unsigned char temp[len];
    net_copy(temp, value, len, ni->endian);
    memcpy(buffer + ni->index, temp, len);
    ni->index = index + len;
}
void NET_CopyLong(unsigned char* buffer, unsigned long value, NET_INDEX* ni)
{
    unsigned index = ni->index;
    int len = 8;
    unsigned char temp[len];
    net_copy(temp, value, len, ni->endian);
    memcpy(buffer + ni->index, temp, len);
    ni->index = index + len;
}