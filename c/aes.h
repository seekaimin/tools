#ifndef __AES_H__
#define __AES_H__
#include "common.h"


//加密解密类型
typedef enum _EN_CRYPT_TYPE
{
	CRYPT_TYPE_ENCRYPT = 0,	//加密
	CRYPT_TYPE_DECRYPT	    //解密
}EN_CRYPT_TYPE;

//算法模式
typedef enum _EN_CRYPT_MODE
{
	CRYPT_MODE_ECB = 0,	//ECB模式
	CRYPT_MODE_CBC,	    //CBC模式
	CRYPT_MODE_CFB128	//不支持
}EN_CRYPT_MODE;

// 填充模式
typedef enum _PADDING_MODE
{
    PADDING_NONE = 0,   // 不填充
	PADDING_ZERO,	    // 填充0
	PADDING_PKCS5,	    // PKCS5
	PADDING_PKCS7,      // PKCS7
}PADDING_MODE;

/**
 * brief          AES context structure
 */
typedef struct _ST_CA_AES_CONTEXT
{
    unsigned int nr;           /*!<  number of rounds  */
    unsigned int *rk;          /*!<  AES round keys    */
    unsigned int buf[68];      /*!<  unaligned data    */
}ST_CA_AES_CONTEXT;

/* 
 * enType		加密解密类型
 * enMode		算法模式
 * padMode		填充模式
 * input		输入数据
 * output		输出数据 申请长度 = inputLen + 16(加密时避免补齐后溢出) 
 * inputLen		输入数据长度
 * outputLen	输出数据长度 计算返回
 * Key			密钥 (16字节密钥)
 * keyLen		密钥长度 
 */
int AES_RunAes(EN_CRYPT_TYPE enType, EN_CRYPT_MODE enMode, PADDING_MODE padMode, unsigned char* input, unsigned char* output, unsigned int inputLen, unsigned int* outputLen, unsigned char* Key,  unsigned char keyLen);
#endif // AES_H

