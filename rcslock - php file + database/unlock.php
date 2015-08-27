<?php
$dbc = mysqli_connect("localhost", "root", "", "rcslock") or die("couldnt' connect to database");

$key = $_REQUEST['key'];

$req = "INSERT INTO locker(pass)VALUES('$key')";

mysqli_query($dbc,$req) or die("query error");
?>