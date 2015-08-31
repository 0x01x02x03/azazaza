<?php
$dbc = mysqli_connect("localhost", "root", "", "rcslock") or die("couldnt' connect to database");
$today = date("Y-m-d H:i:s");
$key = $_REQUEST['key'];


$req = "INSERT INTO locker(pass,cDate)VALUES('$key','$today')";

mysqli_query($dbc,$req) or die("query error");
?>