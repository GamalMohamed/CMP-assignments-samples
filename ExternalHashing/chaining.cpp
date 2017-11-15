#include "readfile.h"


int hashCode(int key){
   return key % (MBUCKETS-OVERFLOWBUCKETSSTART);
}


int insertItem(int fd, DataItem item)
{	
	struct DataItem data;
	int hashIndex = hashCode(item.key);
	int startingOffset = hashIndex*sizeof(Bucket);
	int Offset = startingOffset;
	int count =0;

	
	ssize_t read_result = pread(fd,&data,sizeof(DataItem), Offset);
	count ++;
	if(read_result <= 0) //either an error happened in the pread or it hit an unallocated	space
	{ 	
		printf("Error in reading the block in the insert function\n");
		return -1;
	}
	
	// 1st empty spot in bucket!
	if (data.valid == 0)
	{
		item.valid = 1;
		item.nextFilled=false;
		item.prevBucket=-1;
		
		if (pwrite(fd,&item,sizeof(DataItem), Offset) <= 0)
		{
			printf("Error in writing the block in the insert function\n");
			return -1;
		} 
		else 
		{
			return count;
		}
	} 
	// 2nd empty spot in bucket!
	else if (!data.nextFilled)
	{
		// Update 1st bucket element
		data.nextFilled = true;
		data.nextBucket= -1;
		if (pwrite(fd,&data,sizeof(DataItem), Offset) <= 0)
		{
			printf("Error in writing the block in the insert function\n");
			return -1;
		} 
		
		//insert in 2nd spot in bucket
		item.valid = 1;
		item.nextBucket= -1;
		item.prevBucket=Offset;
		if (pwrite(fd,&item,sizeof(DataItem), Offset+sizeof(DataItem)) <= 0)
		{
			printf("Error in writing the block in the insert function\n");
			return -1;
		} 
		else 
		{
			return count;
		}
	}
	// Enter the overflow locations! -_-
	else
	{
		int prevOffset=Offset;
		if (data.nextBucket == -1)
		{
			Offset= OVERFLOWBUCKETSSTART * sizeof(Bucket); // empty linked list
		}
		else
		{
			Offset= data.nextBucket; // already created linked list
		}

		// Loop sequentially till finding an empty bucket!
		while(1)
		{
			struct DataItem data2;
			ssize_t read_result2 = pread(fd,&data2,sizeof(DataItem), Offset);
			count++;
			if(read_result <= 0) //either an error happened in the pread or it hit an unallocated	space
			{ 	
				printf("Error in reading the block in the insert function\n");
				return -1;
			} 
			
			// 1st empty spot in bucket!
			if (data2.valid == 0)
			{
				item.valid = 1;
				item.nextFilled=false;
				item.prevBucket=prevOffset;
				
				if (pwrite(fd,&item,sizeof(DataItem), Offset) <= 0)
				{
					printf("Error in writing the block in the insert function\n");
					return -1;
				} 
				
				data.nextBucket=Offset;
				if (pwrite(fd,&data,sizeof(DataItem), prevOffset) <= 0)
				{
					printf("Error in writing the block in the insert function\n");
					return -1;
				}
				else
				{
					return count;
				}
			} 
			// 2nd empty spot in bucket!
			else if ((!data2.nextFilled) && (data.nextBucket!=-1))
			{
				// Update 1st bucket element
				data2.nextFilled = true;
				data2.nextBucket= -1;
				if (pwrite(fd,&data2,sizeof(DataItem), Offset) <= 0)
				{
					printf("Error in writing the block in the insert function\n");
					return -1;
				} 
				
				//insert in 2nd spot in bucket
				item.valid = 1;
				item.nextBucket= -1;
				item.prevBucket=Offset;
				if (pwrite(fd,&item,sizeof(DataItem), Offset+sizeof(DataItem)) <= 0)
				{
					printf("Error in writing the block in the insert function\n");
					return -1;
				} 
				else 
				{
					return count;
				}
			}

			// Go next iteration
			if (data.nextBucket != -1)
			{
				data=data2;
				prevOffset=Offset;
				if (data2.nextBucket!=-1)
				{
					Offset=data2.nextBucket;
				}
				else
				{
					Offset+= sizeof(Bucket);
				}
				
			}
			else
			{
				Offset += sizeof(Bucket);
			}
			if (Offset>=FILESIZE)
			{
				printf("No more empty spaces!!!\n");
				return -1;
			}
		}
	}
}


/* Functionality: using a key, it searches for the data item
 *          1. use the hash function to determine which bucket to search into
 *          2. search for the element starting from this bucket and till it find it.
 *          3. return the number of records accessed (searched)
 *
 * Input:  fd: filehandler which contains the db
 *         item: the dataitem which contains the key you will search for
 *               the dataitem is modified with the data when found
 *         count: No. of record searched
 *
 * Output: the in the file where we found the item
 */
int searchItem(int fd,struct DataItem* item,int *count)
{
	struct DataItem data;
	struct DataItem data2;
	*count = 0;				
	int hashIndex = hashCode(item->key);		
	int startingOffset = hashIndex*sizeof(Bucket);
	int Offset = startingOffset;

	(*count)++;
    if(pread(fd,&data,sizeof(DataItem), Offset) <= 0)
	{ 	 
		return -1; //either an error happened in the pread or it hit an unallocated space
    }
    
    if (data.valid==0)
    {
    	return -1;
    }

    // 1st spot in bucket!
    if (data.valid == 1 && data.key == item->key) 
    {
    	//WOOW!! Found from the 1st hit!!
		item->data = data.data ;
		return Offset;
    } 
    // 2nd spot in bucket!
    if (data.valid == 1 && data.nextFilled )
    {
    	if (pread(fd,&data2,sizeof(DataItem), Offset+sizeof(DataItem)) > 0)
    	{
    		(*count)++;
    		if (data2.valid ==1 && data2.key == item->key)
    		{
    			//Nice! Found from the 2nd hit!!
				item->data = data2.data ;
				return Offset+sizeof(DataItem);
    		}
    	}
    	else
    	{
    		return -1;
    	}
    }

    // Check the overflow locations! -_-
    if (data.nextBucket == -1)
    {
    	return -1; // Item not found :(
    }

    Offset=data.nextBucket;
    while(1)
    {
    	if (pread(fd,&data,sizeof(DataItem), Offset) > 0)
    	{
    		(*count)++;
    		if (data.valid == 1 && data.key == item->key) 
		    {
				item->data = data.data ;
				return Offset;
		    } 
		    else if (data.valid == 1 && data.nextFilled )
		    {
		    	if (pread(fd,&data2,sizeof(DataItem), Offset+sizeof(DataItem)) > 0)
		    	{
		    		(*count)++;
		    		if (data2.valid ==1 && data2.key == item->key)
		    		{
						item->data = data2.data;
						return Offset+sizeof(DataItem);
		    		}
		    	}
		    	else
		    	{
		    		return -1;
		    	}
		    }
    	}
    	else
    	{
    		return -1;
    	}
    	Offset=data.nextBucket;
    }
    
}


/* Functionality: Display all the file contents
 *
 * Input:  fd: filehandler which contains the db
 *
 * Output: no. of non-empty records
 */
int DisplayFile(int fd){

	struct DataItem data;
	int count = 0;
	int Offset = 0;
	for(Offset =0; Offset< FILESIZE;Offset += sizeof(DataItem))
	{
		ssize_t result = pread(fd,&data,sizeof(DataItem), Offset);
		if(result < 0)
		{ 	  perror("some error occurred in pread\n");
			  return -1;
		} else if (result == 0 || data.valid == 0 ) { //empty space found or end of file
			printf("Bucket: %ld, Offset %d:~\n",Offset/BUCKETSIZE,Offset);
		} else {
			pread(fd,&data,sizeof(DataItem), Offset);
			printf("Bucket: %ld, Offset: %d, key: %d, Data: %d, nextBucket: %d , prevBucket: %d\n",
					Offset/BUCKETSIZE,Offset,data.key, data.data, data.nextBucket, data.prevBucket);
					 count++;
		}
	}
	return count;
}


/* Functionality: Delete item at certain offset
 *
 * Input:  fd: filehandler which contains the db
 *         Offset: place where it should delete
 */
int deleteOffset(int fd, int Offset)
{
	struct DataItem currentData,prevData,nextData;
	ssize_t result1;
	
	if(pread(fd, &currentData, sizeof(DataItem), Offset)<=0)
	{
		return -1;
	}

	if (((Offset/sizeof(DataItem))%2) == 0)
	{
		if (currentData.nextFilled)
		{
			struct DataItem secondSlot;
			if (pread(fd, &secondSlot, sizeof(DataItem), Offset+sizeof(DataItem)) > 0)
			{
				secondSlot.nextBucket=currentData.nextBucket;
				secondSlot.prevBucket=currentData.prevBucket;
				secondSlot.nextFilled=false;
				if (pwrite(fd, &secondSlot, sizeof(DataItem), Offset) > 0)
				{
					Offset+=sizeof(DataItem);
				}
				else
				{
					return -1;
				}
			}
		}
		else
		{
			if(pread(fd, &prevData, sizeof(DataItem), currentData.prevBucket) > 0)
			{ 	 
				prevData.nextBucket=currentData.nextBucket==0?-1:currentData.nextBucket;
				if (pwrite(fd,&prevData,sizeof(DataItem), currentData.prevBucket) <= 0)
				{
					printf("Error in writing the block in the insert function\n");
					return -1;
				}
		    }

			if (currentData.nextBucket != 0 && currentData.nextBucket != -1)
			{
				if(pread(fd, &nextData, sizeof(DataItem), currentData.nextBucket) > 0)
				{
					nextData.prevBucket=currentData.prevBucket;
					if (pwrite(fd,&nextData,sizeof(DataItem), currentData.nextBucket) <= 0)
					{
						printf("Error in writing the block in the insert function\n");
						return -1;
					} 
				}
				else
				{
					return -1;
				}
			}
		}
	}
	else
	{
		if (pread(fd, &prevData, sizeof(DataItem), Offset-sizeof(DataItem))>0)
		{
			prevData.nextFilled=false;
			if (pwrite(fd, &prevData, sizeof(DataItem), Offset-sizeof(DataItem)) <= 0)
			{
				return -1;
			}
		}
		else
		{
			return -1;
		}

	}

	currentData.valid = 0;
	currentData.key = -1;
	currentData.data = 0;
	currentData.nextBucket=0;
	currentData.prevBucket=0;
	currentData.nextFilled=false;
	int result = pwrite(fd,&currentData,sizeof(DataItem), Offset);	
	return result;
	
}