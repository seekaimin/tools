test : 
	cc -c aes.c -o aes.o
	cc -c datetime.c -o datetime.o
	cc -c test.c -o test.o
	cc datetime.o aes.o test.o -o test

.PHONY : clean    
clean :    
	rm test *.o
