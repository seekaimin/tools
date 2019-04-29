test : 
	cc -c aes.c -o aes.o
	cc -c datetime.c -o datetime.o
	cc -c test.c -o test.o
	cc -c md5.c -o md5.o
	cc datetime.o aes.o test.o md5.o -o test.run

.PHONY : clean    
clean :    
	rm test *.o
