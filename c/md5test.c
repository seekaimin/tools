#include <string.h>
#include <stdio.h>
#include "md5.h"

void main(int argc, char *argv[])
{
	int i;
    unsigned char src[] = "012345689abcdefghijklmnopqrstuvwxyzf;fe;dlfekfkef41fd21f2d1fe54fd21f2d1fw5f4ts2f1sd";
    unsigned int inlen = 83;
    unsigned char input[inlen];
	unsigned char output[16];
    for(i = 0;i<inlen;i++){
        input[i] = src[i];
    } 
    MD5_CALC(input,inlen,output);
	printf("src=%s\nsrc buffer=0x",src);
	for(i=0;i<inlen;i++)
	{
		printf("%02x",input[i]);
	}
    printf("\n");
	printf("md5=0x");
	for(i=0;i<16;i++)
	{
		printf("%02x",output[i]);
	}
    printf("\n");
}