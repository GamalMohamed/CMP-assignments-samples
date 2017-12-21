<div class="sidebar">
	<div id="stitle">Subscribe</div>
	<div>
		<form style="padding:3px;text-align:center;" action="http://feedburner.google.com/fb/a/mailverify" method="post" target="popupwindow" onsubmit="window.open('http://feedburner.google.com/fb/a/mailverify?uri=dummy', 'popupwindow', 'scrollbars=yes,width=550,height=520');return true">
			<p>Your email address:</p>
			<p><input type="text" style="width:140px" name="email"/></p>
			<input type="hidden" value="dummy" name="uri"/>
			<input type="hidden" name="dummy" value="en_US"/>
			<input type="submit" value="Subscribe" />
			<p>
		</form>
	</div>
    
    <div id="stitle">Follow Us!</div>
    <div id="social">
    	<a href="https://www.facebook.com/jimmy.poi95" target="_blank"><img src="images/facebook.png" /></a>
       	<a href="http://www.twitter.com/gamal__mohamed" target="_blank"><img src="images/twitter.jpg" /> </a>
        <a href="http://www.google.com" target="_blank"><img src="images/google.png" />
        <a href="http://www.pinterest.com" target="_blank"><img src="images/pinterest.jpg" /> 
    </div>

   	<div id='stitle'>Recent Stories</div>
	<?php 
		$get_posts = "select * from posts order by 1 DESC LIMIT 0,3";
		
		$run_posts = mysqli_query($con, $get_posts); 
		
		while ($row_posts = mysqli_fetch_array($run_posts)) {
			$post_id = $row_posts['post_id'];
			$post_title = $row_posts['post_title'];
			$post_image = $row_posts['post_image'];

			echo "
			 <div class='recent_posts'>
			<h2><a href='details.php?post=$post_id'>$post_title</a></h2>
			
			<img src='admin/news_images/$post_image' width='100' height='100'/>
			
			
			</div>
			
			<br/>
			<br/>
			";
		}
	?>
</div>