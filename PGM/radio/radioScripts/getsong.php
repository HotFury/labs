<?php
        session_start();
	if(!empty($_SERVER['HTTP_X_REQUESTED_WITH']) && strtolower($_SERVER['HTTP_X_REQUESTED_WITH']) == 'xmlhttprequest')
	{
                $_SESSION['songCount']++;
                $_SESSION['infoRepeat'];
		include_once "./ez_sql_core.php";
		include_once "./ez_sql_mysql.php";
                $db = new ezSQL_mysql('root','','radio','localhost'); 
                if ($_SESSION['songCount'] % $_SESSION['infoRepeat'] == 0)
                {
                    $song = $db->get_row("SELECT * FROM songs where artist = 'information' ");
                }
		else
                {
                    $song = $db->get_row("SELECT * FROM songs where artist != 'information' order by song_id");
                }
                $artist = $song->artist;
                $songname = $song->title;
                $url = $song->url;
                $separator = '|';
		echo $url.$separator.$artist.$separator.$songname;
                if ($artist != 'information')
                {
                    $db->get_row("DELETE FROM songs WHERE artist='$artist'");
                    $db->get_row("INSERT INTO `songs` (`url`, `artist`, `title`) VALUES ('$url', '$artist', '$songname')");
                }
	}
?>