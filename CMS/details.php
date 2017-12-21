<!DOCTYPE HTML>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Details</title>
	<link rel="stylesheet" href="styles/style.css" media="all" /> 
</head>

<body>
	<!--Main Container start-->
	<div class="container">
	    <div class="head">
	    	<a href="index.php"><img id="logo" src="images/logo.jpg" /></a>
	        <img id="banner" align="right" src="images/welcome.jpg" /> 
    	</div>
	    
	    <?php include("includes/navbar.php"); ?>
	    
	  	<!--Content Area start-->
	  	<div class="post_area">
		    <?php 
				if(isset($_GET['post'])){
					$post_id = $_GET['post'];
					$get_posts = "select * from posts where post_id='$post_id'";
					$run_posts = mysqli_query($con, $get_posts); 
					
					while ($row_posts = mysqli_fetch_array($run_posts)){
						$post_title = $row_posts['post_title'];
						$post_date = $row_posts['post_date'];
						$post_author = $row_posts['post_author'];
						$post_image = $row_posts['post_image'];
						$post_content = $row_posts['post_content'];

						echo "
						
						<h2>$post_title</h2>
						
						<span><i>Posted by</i> <b>$post_author</b> &nbsp; On  <b>$post_date</b></span> <span style='color:brown;'></span>
						
						<img src='admin/news_images/$post_image' width='300' height='300'/>
						
						<div>$post_content </div>
						
						
						
						";
						}
					}
				include("includes/comment_form.php");
			?>

			<h2 style='padding-left:10px; padding-top:10px; background:silver;'>Related Posts:</h2>
			<?php 
				if(isset($_GET['post'])){
					
				$post_id = $_GET['post'];
				
				$get_posts = "select * from posts where post_id='$post_id'";
				
				$run_posts = mysqli_query($con, $get_posts); 
				
				while ($row_posts = mysqli_fetch_array($run_posts)){
					$cat_id = $row_posts['category_id'];
					
					$get_related = "select * from posts where category_id='$cat_id' LIMIT 0,4";
					
					$run_related = mysqli_query($con, $get_related); 
					
					while($row_related=mysqli_fetch_array($run_related)){
					
					$post_title = $row_related['post_title'];
					$post_image = $row_related['post_image'];
					
					echo "
					<div id='related'>
					<a href='details.php?post=$post_id'>$post_title</a>
					
					<a href='details.php?post=$post_id'><img src='admin/news_images/$post_image' width='150' height='150'/></a>
					</div>
					
					";
					
					}
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