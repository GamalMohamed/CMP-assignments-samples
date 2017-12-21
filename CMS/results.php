<!DOCTYPE HTML>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Results</title>
	<link rel="stylesheet" href="styles/style.css" media="all" /> 
</head>

<body>
	<div class="container">
	    <div class="head">
	    	<a href="index.php"><img id="logo" src="images/logo.jpg" /></a>
	        <img id="banner" align="right" src="images/welcome.jpg" /> 
    	</div>
	    
	    <?php include("includes/navbar.php"); ?>
	    
	    <!--Content Area start-->
	  	<div class="post_area">
		    <?php 
				if(isset($_GET['search'])){
				
				$get_query = $_GET['search_query'];
				
				if($get_query==''){
					echo "<script>alert('please write a keyword')</script>";
					echo "<script>window.open('index.php','_self')</script>";
					exit();
				}

				$get_posts = "select * from posts where post_keywords like '%$get_query%'";
				
				$run_posts = mysqli_query($con, $get_posts); 
				
				while ($row_posts = mysqli_fetch_array($run_posts)){
					$post_id = $row_posts['post_id'];
					$post_title = $row_posts['post_title'];
					$post_date = $row_posts['post_date'];
					$post_author = $row_posts['post_author'];
					$post_image = $row_posts['post_image'];
					$post_content = substr($row_posts['post_content'],0,300);

					echo "
					
					<h2><a id='ltitle' href='details.php?post=$post_id'>$post_title</a></h2>
					
					<span><i>Posted by</i> <b>$post_author</b> &nbsp; On  <b>$post_date</b></span> <span style='color:brown;'></span>
					
					<img src='admin/news_images/$post_image' width='100' height='100'/>
					
					<div>$post_content <a id='rmlink' href='details.php?post=$post_id'>Read More</a></div><br />
					
					
					
					";

					}
				}
				else 
				
				if(isset($_GET['cat'])){
					
					$cat_id = $_GET['cat'];
					
					$get_posts = "select * from posts where category_id='$cat_id'";
					
					$run_posts = mysqli_query($con,$get_posts); 
					
					while ($row_posts = mysqli_fetch_array($run_posts)){
						$post_id = $row_posts['post_id'];
						$post_title = $row_posts['post_title'];
						$post_date = $row_posts['post_date'];
						$post_author = $row_posts['post_author'];
						$post_image = $row_posts['post_image'];
						$post_content = substr($row_posts['post_content'],0,300);

						echo "
						
						<h2><a id='ltitle' href='details.php?post=$post_id'>$post_title</a></h2>
						
						<span><i>Posted by</i> <b>$post_author</b> &nbsp; On  <b>$post_date</b></span> <span style='color:brown;'></span>
						
						<img src='admin/news_images/$post_image' width='100' height='100'/>
						
						<div>$post_content <a id='rmlink' href='details.php?post=$post_id'>Read More</a></div><br />
						
						
						
						";
						}
				}
			?>
	    </div>
	    <!--Content area Ends-->
	    
	    <?php include("includes/sidebar.php"); ?>
	    
	    <div class="footer_area">
	   		<h2 style="padding:20px; text-align:center">&copy; All Rights Reserved 2017 - Team #20</h2>
	    </div>
</div>
</body>
</html>