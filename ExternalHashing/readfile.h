#ifndef READFILE_H_
#define READFILE_H_

#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>
#include <stdio.h>


#define MBUCKETS  10					//Number of BUCKETS
#define RECORDSPERBUCKET 2				//No. of records inside each Bucket
#define BUCKETSIZE sizeof(Bucket)		//Size of the bucket (in bytes)
#define OVERFLOWBUCKETSSTART 5
#define FILESIZE BUCKETSIZE*MBUCKETS    //Size of the file 


//Data Record inside the file
struct DataItem {
   int valid;    // 0 = invalid record, 1 = valid record
   int data;     
   int key;
   int nextBucket; // Points to next bucket
   int prevBucket;
   bool nextFilled;
};


//Each bucket contains number of records
struct Bucket {
	struct DataItem dataItem[RECORDSPERBUCKET];
};

int createFile(int size, char *);

int deleteItem(int key);
int insertItem(int fd,DataItem item);
int DisplayFile(int fd);
int deleteOffset(int filehandle, int Offset);
int searchItem(int filehandle,struct DataItem* item,int *count);




#endif /* READFILE_H_ */
