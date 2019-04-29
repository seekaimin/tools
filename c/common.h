#ifndef __COMMON_H__
#define __COMMON_H__


typedef	char INT8;
typedef unsigned char UINT8;
typedef	short INT16;
typedef unsigned short UINT16;
typedef	int INT32;
typedef unsigned int UINT32;
typedef	long INT64;
typedef unsigned long UINT64;

typedef	int BOOL;
#define	TRUE	1
#define	FALSE	0

 
#undef NULL 
#if defined(__cplusplus)
#define NULL 0
#else
#define NULL ((void *)0)
#endif




#endif // COMMON

