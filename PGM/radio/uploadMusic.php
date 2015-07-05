<html xmlns='http://www.w3.org/1999/xhtml' lang='en' xml:lang='en'>
<head>
    <title>HotFury radio</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    
</head>
<body>
    <?php
        echo "<form name = 'uplSongs' method = 'post' action = addMusicToBase.php>";
        $id = 0;
        $uploaddir = './music/';
        $uploadfile = array();
        $count = sizeof($_FILES['songs']['name']);
        echo "<input type = 'hidden' name = 'count' value=$count>";
        foreach ($_FILES['songs']['name'] as $temp)
        {
            $temp = str_replace("'", "`", $temp);
            echo "<input type = 'hidden' name = 'songFullName$id' value='$temp'>";
            $uploadfile[] = $uploaddir.basename($temp);
            $name = str_replace(".mp3", "", $temp);
            $artist_song = preg_split("/-/", $name);       
            echo "
                    $temp <br>                    
                    artist:<input type = 'text' value = '$artist_song[0]' name = 'artist$id' id = 'artist$id'>
                    song:<input type = 'text' value = '$artist_song[1]' name = 'song$id' id = 'song$id'>
                    <input type = 'button' value = 'Swap' onclick='onSwap($id)' id = $id> <br><br>
                 ";
            $id++;
        }
        $i = 0;
        foreach ($_FILES['songs']['tmp_name'] as $temp)
        {
            copy($temp, $uploadfile[$i]);
            $i++;
        }
        echo "Information repeat period: <input type='text' name='infoRepeat'>";
        echo "<input type='submit' value='Add to base'>";
        echo "</form>";
        echo "  <script>
                    function onSwap(id)
                    {
                        var artist = document.getElementById('artist'+id);
                        var song = document.getElementById('song'+id);
                        var artistVal = artist.value;
                        var songVal = song.value
                        artist.setAttribute('value', songVal);
                        song.setAttribute('value', artistVal);
                        return true;
                    }
                </script>";
    ?>
</body>
</html>
