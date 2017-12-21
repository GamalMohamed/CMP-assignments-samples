<div class="navbar">
    <ul id="menu">
      <?php 
		  include("includes/database.php");
		  
		  $get_cats = "select * from categories";
		  
		  $run_cats = mysqli_query($con, $get_cats);
		  
		  while ($cats_row=mysqli_fetch_array($run_cats)){
			  $cat_id=$cats_row['cat_id']; 
			  $cat_title=$cats_row['cat_title'];
			  
			  echo "<li><a href='index.php?cat=$cat_id'>$cat_title</a></li>";
			}
	  ?>  
    </ul>
        
    <div id="form">
    	<form method="get" action="results.php" enctype="multipart/form-data"> 
        	<input type="text" name="search_query" /> 
            <input type="submit" name="search" value="Search" />
        </form>
    </div>
</div>