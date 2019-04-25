#include <time.h>
#include "datetime.h"


char is_leap_year(int year) {
	if(
		(year % 400 == 0) 
		|| (year % 4 == 0 && year % 100 != 0)
		){
		return 1;
	}else{
		return 0;
	}
}

int days_of_month(int year, int month) {
	int days = 0;
	if (month < 1 || month > 12) {
		//error
	} else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
		// 大
		days = 31;
	} else if (month == 2) {
		if (is_leap_year(year)) {
			days = 29;
		} else {
			days = 28;
		}
	} else {
		days = 30;
	}
	return days;
}
int diff_month_days(int year, int start, int end) {
	int days = 0;
	int month = start;
	while (month < end) {
		days += days_of_month(year, month);
		month++;
	}
	return days;
}
int diff_year_days(int start, int end) {
	int days = 0;
	int year = start;
	while (year <= (end - 1)) {
		if (is_leap_year(year)) {
			days += DAYS_OF_LEAP_YEAR;
		} else {
			days += DAYS_OF_COMMON_YEAR;
		}
		year++;
	}
	return days;
}
long toUTC(int year, int month, int day, int hour, int minute, int second, int offset) {
	long year_days = 0;
	long month_days = 0;
	long days = 0;
	long hours = 0;
	long minutes = 0;
	long seconds = 0;
	year_days = diff_year_days(1970, year);
	month_days = diff_month_days(year, 1, month);
	days = day - 1;
	hours = hour;
	minutes = minute;
	seconds = second;
	// 之前月份所有天数的秒数
	long seconds_days = SECONDS_OF_DAY * (year_days + month_days + days) + SECONDS_OF_HOUR * hours
			+ SECONDS_OF_MINUTE * minutes + seconds;
	// 计算偏移量
	seconds_days = seconds_days - (offset * SECONDS_OF_HOUR);
	return seconds_days;
}

struct tm new_date(int year, int month, int day, int hour, int minute, int second){
	struct tm date;    
	date.tm_year = year - 1900;
	date.tm_mon = month - 1;
	date.tm_mday = day;
	date.tm_hour = hour;
	date.tm_min = minute;
	date.tm_sec = second;
    return date;
}

long date2utc(struct tm* date,int offset) { 
	long utc = mktime(date);
	utc = utc - offset * 3600;
	return utc;
}

struct tm* utc2date(long utc, int offset){
	struct tm *date;
	utc = utc + offset * 3600;
	date = localtime(&utc); 
	return date;
}
