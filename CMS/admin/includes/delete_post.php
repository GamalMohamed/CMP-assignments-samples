<?php 
include("database.php"); 

if(isset($_GET['delete_post'])){
	
	$delete_id = $_GET['delete_post'];
	
	$delete_post = "delete from posts where post_id='$delete_id'";
	
	$run_delete = mysqli_query($con,$delete_post); 
	
	echo "<script>alert('A post has been deleted')</script>";
	
	echo "<script>window.open('../index.php?view_posts','_self')</script>";

	}
?>