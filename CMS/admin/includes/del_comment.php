<?php 
@session_start(); 

if(!isset($_SESSION['user_name'])){
	
	echo "<script>window.open('../login.php?not_authorize=You are not Authorize to access!','_self')</script>";
	}

else {

?>

<?php 
include("database.php");
	
if(isset($_GET['del_comment'])){
	
	$delete_id = $_GET['del_comment']; 
	
	$delete_comment = "delete from comments where comment_id='$delete_id'";
	
	$run_delete = mysqli_query($con,$delete_comment); 
	
	echo "<script>alert('A comment was deleted!')</script>";
	echo "<script>window.open('index.php?view_comments','_self')</script>";

	}
?>

<?php } ?>