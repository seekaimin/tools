#ifndef __DATETIME_H__
#define __DATETIME_H__

#define	SECONDS_OF_MINUTE  60
#define	MINUTES_OF_HOUR  60
#define	SECONDS_OF_HOUR SECONDS_OF_MINUTE * MINUTES_OF_HOUR
#define	HOURS_OF_DAY  24
#define	SECONDS_OF_DAY HOURS_OF_DAY * SECONDS_OF_HOUR
#define	MONTH_OF_YEAR  12
#define	DAYS_OF_COMMON_YEAR  365
#define	DAYS_OF_LEAP_YEAR  366



struct tm new_date(int year, int month, int day, int hour, int minute, int second);
long date2utc(struct tm* date,int offset);
struct tm* utc2date(long utc, int offset);

#endif

