#include <stdio.h>
#include <string.h>
#include <netinet/in.h>
#include <sys/socket.h>

#include "tcp.h"
//return 
//  -1:创建socket失败
//  -2:端口冲突
int tcp_create(int port,int backlog){
	int sockfd;/*socket句柄和建立连接后的句柄*/
	struct sockaddr_in my_addr;/*本方地址信息结构体，下面有具体的属性赋值*/
	sockfd=socket(AF_INET,SOCK_STREAM,0);//建立socket 
	if(sockfd==-1){
		return -1;
	}
	my_addr.sin_family=AF_INET;/*该属性表示接收本机或其他机器传输*/
	my_addr.sin_port=htons(port);/*端口号*/
	my_addr.sin_addr.s_addr=htonl(INADDR_ANY);/*IP，括号内容表示本机IP*/
	bzero(&(my_addr.sin_zero),8);/*将其他属性置0*/
	if(bind(sockfd,(struct sockaddr*)&my_addr,sizeof(struct sockaddr))<0){//绑定地址结构体和socket
		return -2;
	}
    listen(sockfd,backlog);//开启监听 ，第二个参数是最大监听数 
    return sockfd;
}



TCP_CLIENT tcp_accept(int server_handle){
	TCP_CLIENT client;//客户端信息
	int sin_size = sizeof(struct sockaddr_in);
	int c = accept(server_handle,(struct sockaddr*)&client.addr,&sin_size);//在这里阻塞知道接收到消息，参数分别是socket句柄，接收到的地址信息以及大小 
	if(c != -1){
		client.handle = c;
		client.addr_size = sin_size;
	}
	return client;
}