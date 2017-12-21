<?php 
	session_start();
?>

<!DOCTYPE HTML>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Admin Login</title>
	<link rel="stylesheet" href="style_login.css" media="all" /> 
</head>

<body>
	<div class="login">
		<h1>Admin Login</h1>
	    <form method="post" action="login.php">
	    	<input type="text" name="user_name" placeholder="Username" required="required" />
	        <input type="password" name="user_pass" placeholder="Password" required="required" />
	       
	        <input type="submit" class="btn btn-primary btn-block btn-large" name="login" value="Admin Login" />
	</form>
	</div>

	<h2 style="color:#FFF; text-align:center"><?php echo @$_GET['not_authorize']; ?></h2>
</body>

</html>

<?php	
	include("includes/database.php");

	if(isset($_POST['login'])){
		
		$user_name = mysqli_real_escape_string($con,$_POST['user_name']);
		$user_pass = mysqli_real_escape_string($con,$_POST['user_pass']);
		
		$select_user = "select * from users where user_name='$user_name' AND user_password='$user_pass'";
		
		$run_user = mysqli_query($con,$select_user);
		
		
		if(mysqli_num_rows($run_user) > 0) {
			$_SESSION['user_name']=$user_name;
			echo "<script>window.open('index.php?logged=You have Successfully Logged In!','_self')</script>";	
		}

		else {
			echo "<script>alert('User Name or Password is incorrect')</script>";
		}
	}
?>