<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#14</title>
    </head>
    <body>      
        <?php
            $imHeight = 640;
            $imWidth = 1024;
            $multiplayer = 10;
            $gridSize = 15;
            for ($i =  0; $i < 30; $i++)
            {
                $x[] = $_POST["x$i"];
                $y[] = $_POST["y$i"];
            }
            $image = imagecreate($imWidth, $imHeight);
            $fillColor = imagecolorallocate($image, 255, 255, 255);
            imagefill ($image, 0,0,$fillColor);
            $lineColor = imagecolorallocate($image, 255, 0, 255); 
            $gridColor = imagecolorallocate($image, 122, 122, 122); 
            for ($i = 0; $i < $imWidth; $i += $gridSize)
            {
                imageline($image, $i, 0, $i, $imHeight, $gridColor);
            }
            for ($i = 0; $i < $imHeight; $i += $gridSize)
            {
                imageline($image, 0, $i, $imWidth, $i, $gridColor);
            }
            for ($i = 0; $i < sizeof($x) - 1; $i++)
            {
                imageline($image, (int)$x[$i], $imHeight - (int)$y[$i] * $multiplayer, (int)$x[$i+1], $imHeight - (int)$y[$i+1] *$multiplayer,$lineColor);
            }
            imagecolordeallocate($image,$color); 
            imagejpeg($image, './img.jpg');
        ?>
        <img src="img.jpg"/>
     </body>
</html>