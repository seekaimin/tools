#include <string.h>
#include <stdio.h>
#include "net.h"


void main(){
    int i = 0,size = 100;
    unsigned char buffer[size];
    memset(&buffer,0,size);
    NET_INDEX index;
    //写入
    index.index = 0;
    index.endian = NET_BIG_ENDIAN;

    NET_CopyChar(buffer,10,&index);
    NET_CopyShort(buffer,65531,&index);
    NET_CopyInt(buffer,123456789,&index);
    NET_CopyLong(buffer,987654321,&index);

    printf("index=%d    endian=%d\n",index.index,index.endian);
    for(i = 0;i<index.index;i++)
    {
        printf("%02X ",buffer[i]);
    }
    printf("\n");
    //
    index.index = 0;
    index.endian = NET_BIG_ENDIAN;
    
    unsigned long v = 0;
    v = NET_GetChar(buffer,&index);
    printf("val=%ld---index=%d\n",v,index.index);
    v = NET_GetShort(buffer,&index);
    printf("val=%ld---index=%d\n",v,index.index);
    v = NET_GetInt(buffer,&index);
    printf("val=%ld---index=%d\n",v,index.index);
    v = NET_GetLong(buffer,&index);
    printf("val=%ld---index=%d\n",v,index.index);
    printf("index=%d    endian=%d\n",index.index,index.endian);
    for(i = 0;i<index.index;i++)
    {
        printf("%02X ",buffer[i]);
    }
    printf("\n");
}


