#include "readfile.h"

/* First hash function to choose bucket
 * Input: key used to calculate the hash
 * Output: HashValue;
 */
int FirstHashCode(int key){
    return key % MBUCKETS;
 }

 /* Second hash function to choose bucket
 * Input: key used to calculate the hash
 * Output: HashValue;
 */
int SecondHashCode(int key){
    return key % (MBUCKETS - 3);
}

int insertItem(int fd,DataItem item){
	
    struct DataItem data;
    //Using the first hashing functions
	int hashIndex = FirstHashCode(item.key);
	int startingOffset = hashIndex*sizeof(Bucket);	
	int count =0;

    ssize_t read_result = pread(fd,&data,sizeof(DataItem),startingOffset);
    count ++;
    if(read_result <= 0) //either an error happened in the pread or it hit an unallocated	space
    { 	
        printf("Error in reading the block in the insert function");
        return -1;
    }

    if (data.valid == 0){
        //empty spot !!!
        item.valid = 1;
        ssize_t write_result = pwrite(fd,&item,sizeof(DataItem),startingOffset);
        
        if (write_result <= 0){
            printf("Error in writing the block in the insert function");
            return -1;
        } else{
            return count;
        }
    }else{
        hashIndex = SecondHashCode(item.key);
        startingOffset = hashIndex*sizeof(Bucket);
        int Offset = startingOffset;
        int rewind =0;

        loop:
            read_result = pread(fd,&data,sizeof(DataItem), Offset);
            count ++;
            if(read_result <= 0) //either an error happened in the pread or it hit an unallocated	space
            { 	
                printf("Error in reading the block in the insert function");
                return -1;
            }
        
            if (data.valid == 0){
                //empty spot !!!
                item.valid = 1;
                ssize_t write_result = pwrite(fd,&item,sizeof(DataItem), Offset);
                
                if (write_result <= 0){
                    printf("Error in writing the block in the insert function");
                    return -1;
                } else{
                    return count;
                }

            } else{
                Offset +=sizeof(DataItem);  //move the offset to next record
                if(Offset >= FILESIZE && rewind ==0 )
                { //if reached end of the file start again
                        rewind = 1;
                        Offset = 0;
                        goto loop;
                } else
                    if(rewind == 1 && Offset >= startingOffset) {
                        printf("Can't insert: No empty spaces");
                        return -1; //no empty spaces
                }
                goto loop;
            }
        }
}

int searchItem(int fd,struct DataItem* item,int *count)
{

	//Definitions
	struct DataItem data;   //a variable to read in it the records from the db
	*count = 0;				//No of accessed records
	int rewind = 0;			//A flag to start searching from the first bucket
	int hashIndex = FirstHashCode(item->key);  				//calculate the Bucket index
	int startingOffset = hashIndex*sizeof(Bucket);		//calculate the starting address of the bucket
	
	//on the linux terminal use man pread to check the function manual
	ssize_t result = pread(fd,&data,sizeof(DataItem),startingOffset);
	//one record accessed
	(*count)++;
	//check whether it is a valid record or not
    if(result <= 0) //either an error happened in the pread or it hit an unallocated space
	{ 	 // perror("some error occurred in pread");
		  return -1;
    }
    else if (data.valid == 1 && data.key == item->key) {
    	//I found the needed record
    		item->data = data.data ;
    		return startingOffset;

    } else { //not the record I am looking for
        hashIndex = FirstHashCode(item->key);
        startingOffset = hashIndex*sizeof(Bucket);
        int Offset = startingOffset;

        RESEEK:
        //on the linux terminal use man pread to check the function manual
        result = pread(fd,&data,sizeof(DataItem),Offset);
        //one record accessed
        (*count)++;
        //check whether it is a valid record or not
        if(result <= 0) //either an error happened in the pread or it hit an unallocated space
        { 	 // perror("some error occurred in pread");
              return -1;
        }
        else if (data.valid == 1 && data.key == item->key) {
            //I found the needed record
                item->data = data.data ;
                return startingOffset;
        } else {
    		Offset +=sizeof(DataItem);  //move the offset to next record
    		if(Offset >= FILESIZE && rewind ==0 )
    		 { //if reached end of the file start again
    			rewind = 1;
    			Offset = 0;
    			goto RESEEK;
    	     } else
    	    	if(rewind == 1 && Offset >= startingOffset) {
    			    return -1; //no empty spaces
    	     }
            goto RESEEK;
        }
    }
}

int DisplayFile(int fd){
    
    struct DataItem data;
    int count = 0;
    int Offset = 0;
    for(Offset =0; Offset< FILESIZE;Offset += sizeof(DataItem))
    {
        ssize_t result = pread(fd,&data,sizeof(DataItem), Offset);
        if(result < 0)
        { 	  perror("some error occurred in pread");
              return -1;
        } else if (result == 0 || data.valid == 0 ) { //empty space found or end of file
            printf("Bucket: %ld, Offset %d:~\n",Offset/BUCKETSIZE,Offset);
        } else {
            pread(fd,&data,sizeof(DataItem), Offset);
            printf("Bucket: %ld, Offset: %d, Data: %d, key: %d\n",Offset/BUCKETSIZE,Offset,     data.data,data.key);
            count++;
        }
    }
    return count;
}

int deleteOffset(int fd, int Offset)
{
	struct DataItem dummyItem;
	dummyItem.valid = 0;
	dummyItem.key = -1;
	dummyItem.data = 0;
	int result = pwrite(fd,&dummyItem,sizeof(DataItem), Offset);
	return result;
}