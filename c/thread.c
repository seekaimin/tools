#include <stdio.h>
#include <unistd.h>
#include <pthread.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <sys/socket.h>
#include <sys/wait.h>


void a_run(void *arg)
{
    printf("args=%d\n" ,*(int*)arg);
    for(int i = 0;i<10;i++){
        printf("run d=%d \n",i);
        usleep(1*1000*1000);
    }
}

void main()
{
    long thread_id;
    int arg = 10;
    int ret = pthread_create(&thread_id, NULL, a_run, &arg );
    if( ret != 0 ){
        printf( "Create thread error!\n");
    }
    printf("thread id=%ld\n",thread_id);
    for(int i=0;i<10;i++){
        printf("main d=%d \n",i);
        usleep(1*1000*1000);
    }
}