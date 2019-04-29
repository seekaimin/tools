#ifndef __CRC_H__
#define __CRC_H__


/**
* 计算 CRC32
* @param buff 需要计算的源
* @param size 数据长度
* @return CRC32
*/
unsigned int CRC_CRC32(unsigned char* buff, unsigned int size);

#endif