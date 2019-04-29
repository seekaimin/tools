#include <string.h>
#include <stdio.h>
#include <time.h>
#include "common.h"
#include "datetime.h"

void main(){
	//2019-04-25 02:51:01  1556131861
	printf("src date =2019-02-25 02:51:01 \n");
	int year = 2019, month = 4, day = 25, hour = 02, mintue = 51, second = 1,offset= 8; //offset 时区:小时偏移量  北京=8
	struct tm date;
	date = new_date(year, month, day, hour, mintue, second);	
	struct tm* dt;
	dt = &date;
	printf("utc date = %d-%d-%d %d:%d:%d\n",dt->tm_year+1990,dt->tm_mon-1,dt->tm_mday,dt->tm_hour,dt->tm_min,dt->tm_sec);
	long utc = date2utc(&date, offset);
	printf("utc = %ld\n",utc);
	memset(dt,0,sizeof(struct tm));
	dt = utc2date(utc, offset);
	printf("date = %d-%d-%d %d:%d:%d\n",dt->tm_year+1990,dt->tm_mon-1,dt->tm_mday,dt->tm_hour,dt->tm_min,dt->tm_sec);
}
