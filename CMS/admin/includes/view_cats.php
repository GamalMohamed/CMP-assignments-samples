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
<title>All Categories</title>

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
    	<td colspan="8" align="center" bgcolor="#0099CC"><h2>View all posts here</h2></td>
    </tr>
	
    	<tr>
        	<th>ID</th>
            <th>Title</th>
            <th>Edit</th>
            <th>Delete</th>
        </tr>

<?php 
include("includes/database.php");

	$get_cats = "select * from categories";
	
	$run_cats = mysqli_query($con,$get_cats); 
	
	$i=1;
	
	while ($row_cats = mysqli_fetch_array($run_cats)){
		
			
			$cat_id = $row_cats['cat_id'];
			$cat_title = $row_cats['cat_title'];


?>
		<tr align="center">
        	<td><?php echo $i++; ?></td>
            <td><?php echo $cat_title; ?></td>
            <td><a href="index.php?edit_cat=<?php echo $cat_id; ?>">Edit</a></td>
            <td><a href="index.php?delete_cat=<?php echo $cat_id; ?>">Delete</a></td>
        </tr>

<?php } ?>

</table>
</body>
</html>

<?php } ?>