<?php
        header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#9</title>
    </head>
    <body>
        <div> 
            Лабораторная работа №9 <br>
            10.05.2015 <br>
            Тема: Работа с FTP-серверами средствами PHP. <br>
            Задание: Найти и отобразить все файлы FTP-сервера, имеющие одинаковую длину. <br>
            Выполнил: Кононов Александр. Группа КИТ-20б <br> <br>
        </div>
        <?php
            $ftp_stream = ftp_connect("127.0.0.1", 21);
            if (ftp_login($ftp_stream, "HotFury", "0000")) 
            {
                echo "Подключение удачно<br>";
                $files = \ftp_rawlist($ftp_stream, "");
                echo "Файлы на ftp-сервере c одинаковым размером:<br>";
                for ($i = 0; $i < sizeof($files); $i++)
                {
                    $info = preg_split("/[\s,]+/", $files[$i]);
                    $fileSize[$i] =  $info[4];
                    for ($j = 8; $j < sizeof($info); $j++)
                    {
                        $fileName[$i] .= $info[$j];
                    }
                }
                for ($i = 0; $i < sizeof($fileSize); $i++)
                {
                    for ($j = $i + 1; $j < sizeof($fileSize); $j++)
                    {
                        if ($fileSize[$i] == $fileSize[$j])
                        {
                            $equalSizeName[] = $fileName[$i];
                            $equalSizeName[] = $fileName[$j];
                            $equalSizeValue[] = $fileSize[$i];
                            $equalSizeValue[] = $fileSize[$j];
                        }
                    }
                }
                echo "<div style='float: left'>";
                foreach ($equalSizeName as $name)
                {
                    echo $name . "<br>";
                }
                echo "</div>";
                echo "<div>";
                foreach ($equalSizeValue as $size)
                {
                    echo "|".$size . "<br>";
                }
                echo "</div>";
            }
            else
            {
                echo "Подключение не удалось";
            }
            ftp_close($ftp_stream);
        ?>
     </body>
</html>