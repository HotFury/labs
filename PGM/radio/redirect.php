<html xmlns='http://www.w3.org/1999/xhtml' lang='en' xml:lang='en'>
<head>
    <title>HotFury radio</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    
</head>
<body>
    <?php
        if ($_POST["visitor"] == "admin")
        {
            echo "Welcome, Admin <br>";
            echo "<form enctype='multipart/form-data' method='post' action='uploadMusic.php'>
                    <p><input type='file' multiple accept='audio/mp3' name='songs[]'>
                    <input type='submit' value='Upload'></p>
                  </form> ";
        }
        else 
        {
            echo 'Hello, user <br>';
            echo '<a href="http://localhost/radio/radio.php" target="_blank">Go to radio</a>';   
        }
    ?>
</body>
</html>

