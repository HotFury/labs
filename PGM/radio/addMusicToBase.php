<?php
    session_start();
?>
<html xmlns='http://www.w3.org/1999/xhtml' lang='en' xml:lang='en'>
<head>
    <title>HotFury radio</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    
</head>
<body>
    <?php
        
        $_SESSION['infoRepeat'] = $_POST['infoRepeat'] + 1;
        include_once "./radioScripts/ez_sql_core.php";
        include_once "./radioScripts/ez_sql_mysql.php";
        $db = new ezSQL_mysql('root','','radio','localhost'); 
        $count = $_POST['count'];
        $url = "http://localhost/radio/music/";

        for ($i = 0; $i < $count; $i++)
        {
            $artist = $_POST['artist'.$i];
            $song = $_POST['song'.$i];
            $songFullName = $_POST['songFullName'.$i];
            $db->get_row("INSERT INTO `songs` (`url`, `artist`, `title`) VALUES ('$url$songFullName', '$artist', '$song')");
        }
        echo "Database updated successfull <br>";
        echo '<a href="http://localhost/radio/" target="_blank">MainPage</a>'; 
    ?>
</body>
</html>
