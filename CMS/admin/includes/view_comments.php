<?php 
@session_start(); 

if(!isset($_SESSION['user_name'])){
	
	echo "<script>window.open('../login.php?not_authorize=You are not Authorize to access!','_self')</script>";
	}

else {


?>
<!DOCTYPE HTML>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>All Comments</title>

	<style type="text/css">
	th,td,tr,table {padding:0; margin:0;}

	th {border-left:2px solid #333; border-bottom:3px solid #333;}

	td { border-left:2px solid #999;}

	h2 {padding:10px;}

	</style>
</head>

<body>
<table align="center" bgcolor="#FF9999" width="780"> 
	
    <tr>
    	<td colspan="8" align="center" bgcolor="#0099CC"><h2>Manage Comments Here:</h2></td>
    </tr>
	
	<tr>
    	<th>ID</th>
        <th>Comment</th>
        <th>Name</th>
        <th>Email</th>
        <th>Status</th>
        <th>Delete</th>
    </tr>

<?php 
	include("includes/database.php");

	$get_comments = "select * from comments";
	
	$run_comments = mysqli_query($con,$get_comments); 
	
	$i=1;
	
	while ($row_comments = mysqli_fetch_array($run_comments)){
		$comment_id = $row_comments['comment_id'];
		$comment_name = $row_comments['comment_name'];
		$comment_email = $row_comments['comment_email'];
		$comment_text = $row_comments['comment_text'];
		$status = $row_comments['status'];
?>
	<tr align="center">
    	<td><?php echo $i++; ?></td>
        <td><?php echo $comment_text; ?></td>
        <td><?php echo $comment_name; ?></td>
        <td><?php echo $comment_email; ?></td>
           
       <td>
           <?php 
		   
		   if($status=='approve')
		   {
			   echo "<a href='index.php?unapprove=$comment_id'>Unapprove</a>";
			   
			}
		   else {
			   echo "<a href='index.php?approve=$comment_id'>Approve</a>";
			   
			}
           ?>
       </td>
        
       <td><a href="index.php?del_comment=<?php echo $comment_id; ?>">Delete</a></td>
    </tr>

<?php } ?>
</table>
</body>
</html>
<?php } ?>