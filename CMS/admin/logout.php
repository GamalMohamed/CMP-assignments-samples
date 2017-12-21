<?php 
	session_start(); 

	session_destroy();

	echo "<script>window.open('login.php?logout=Your Successfully Logged Out!','_self')</script>";
?>