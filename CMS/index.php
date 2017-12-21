<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Home</title>
    <link rel="stylesheet" href="styles/style.css" media="all" /> 
</head>

<body>
<div class="container">
    
    <div class="head">
    	<a href="index.php"><img id="logo" src="images/logo.jpg" /></a>
        <img id="banner" align="right" src="images/welcome.jpg" /> 
    </div>
    
    <?php include("includes/navbar.php"); ?>

    <div class="content_wrapper">

        <?php include("includes/post_content.php"); ?>
        
        <?php include("includes/sidebar.php"); ?>
    
    </div>
    
    
    <div class="footer_area">
        <h2 style="padding:20px; text-align:center">&copy; All Rights Reserved 2017 - Team #20</h2>
    </div>

</div>
</body>
</html>