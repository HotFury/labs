<?php
    header("Content-Type: text/html; charset=utf-8");
    $client = 'anyone';
    if (isset($_COOKIE[$client]))
    {
        $cnt = $_COOKIE[$client] + 1;
    }
    else
    {
        $cnt = 0;
    }
    $seconds = 2 * 3600;
    $time = mktime(0, 0, 0, gmdate("m"), gmdate("d")+1, gmdate("Y")) - time() - $seconds;
    setcookie($client,$cnt,time()+$time);
?>
<html>
    <head>
        <title>lab#10</title>
    </head>
    <body>
        <div> 
            Страница №2 <br>
            <a href="index.php">главная страница</a> <a href="page1.php">страница 1</a> <br><br>
            <a href="statistic.php">статистика посещений</a>
        </div>
     </body>
</html>