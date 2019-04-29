#include <stdio.h>
#include <unistd.h>
#include <string.h>
#include <errno.h>

#include "tcp.h"


int main() {
	int sockfd = tcp_create(6666,5);
	if(sockfd == -1){
		printf("socket failed:%d \n",errno);
		return -1;
	}
    printf("started id=%d\n", sockfd);
    while(sockfd!=-1){
        TCP_CLIENT client;
		client = tcp_accept(sockfd);
        if(client.handle == -1){
            printf("receive failed\n");
			break;
        } else{
            printf("receive success\n");
            send(client.handle,"Hello World!\n",12,0);//发送内容，参数分别是连接句柄，内容，大小，其他信息（设为0即可） 
			close(client.handle);
        }
    }
    printf(" exit !\n");
	close(sockfd);
	return 0;
}
