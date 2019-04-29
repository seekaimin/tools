#ifndef __TCP_H__
#define __TCP_H__

#include <netinet/in.h>

//加密解密类型
typedef struct _TCP_CLIENT
{
    ///客户端句柄
	int     handle;	  
    //记录客户端信息长度  
    int     addr_size;
    //客户端信息
	struct  sockaddr_in addr;
}TCP_CLIENT;



//
//创建socket
//  port    :   监听端口
//  backlog :   排队数量
//return 
//  -1   :   创建socket失败
//  -2   :   端口冲突
//  other:   监听socket句柄
int tcp_create(int port,int backlog);
//accept 新客户端接入
//参数:
//  ser:server句柄
//return:
//  新接入的客户端 
TCP_CLIENT tcp_accept(int ser);
#endif // COMMON