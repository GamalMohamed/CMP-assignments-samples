<?php 
session_start(); 

if(!isset($_SESSION['user_name'])){
	
	echo "<script>window.open('login.php?not_authorize=You are not Authorize to access!','_self')</script>";
	}

else {


?>

<!DOCTYPE HTML>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Admin Dashboard</title>
	<link rel="stylesheet" href="style.css" />
</head>

<body>
	<div class="wrapper">
	   <a href="index.php"> <div class="header"></div></a>
	    <div class="left">
	    	<h3 style="padding:5px;">Manage Content</h3>
			<a href="index.php?insert_post">Insert New Post</a>
			<a href="index.php?view_posts">View all Posts</a>
			<a href="index.php?insert_cat">Insert New Category</a>
			<a href="index.php?view_cats">View all Categories</a>
			<a href="index.php?view_comments">View all Comments</a>
			<a href="logout.php">Admin Logout</a>
	    </div>
	    
	    <div class="right">
	    
	   	<span style="padding:5px; margin:5px;"> <strong>Notifications:</strong><span style="color:red; padding:5px; margin:5px;"><a href="index.php?view_comments">
	   
	   Pending Comments 
	   <?php 
		   include("includes/database.php"); 
		   
		   $get_comments = "select * from comments where status='unapprove'";
		   
		   $run_comments = mysqli_query($con,$get_comments); 
		   
		   $count = mysqli_num_rows($run_comments);
		   
		   echo "(" . $count . ")";
	   ?>

	   </a></span></span>
	 <h2 style="color:#C33;"><?php echo @$_GET['logged']; ?></h2>
	 <br/>
	 <div align="middle">
	 	 <span style="font-size:18px;"><strong>Active User:</strong></span><h2 style="color:#03C;"><?php echo $_SESSION['user_name']; ?></h2>
	</div>
	 <?php 
			if(isset($_GET['insert_post'])){
			
			include("includes/insert_post.php");
			
			}
			if(isset($_GET['view_posts'])){
			
			include("includes/view_posts.php");
			
			}
			
			if(isset($_GET['edit_post'])){
			
			include("includes/edit_post.php");
			
			}
			
			if(isset($_GET['insert_cat'])){
			
			include("includes/insert_cat.php");
			
			}	
			
			if(isset($_GET['view_cats'])){
			
			include("includes/view_cats.php");
			
			}	
			
			if(isset($_GET['delete_cat'])){
			
			include("includes/delete_cat.php");
			
			}	
			
			if(isset($_GET['edit_cat'])){
			
			include("includes/edit_cat.php");
			
			}	
			
			if(isset($_GET['view_comments'])){
			
			include("includes/view_comments.php");
			
			}	
			
			if(isset($_GET['unapprove'])){
			
			include("includes/comment_status.php");
			
			}	
			
			if(isset($_GET['approve'])){
			
			include("includes/comment_status.php");
			
			}	
			
			if(isset($_GET['del_comment'])){
			
			include("includes/del_comment.php");
			
			}	
	 ?>
	    </div>
	</div>
</body>
</html>

<?php } ?> 