<?php
    header("Content-Type: text/html; charset=utf-8");
    $client = 'anyone';
?>
<html>
    <head>
        <title>lab#10</title>
    </head>
    <body>
        <div> 
            Лабораторная работа №10 <br>
            10.05.2015 <br>
            Тема: использование сесcий и cookies в PHP. <br>
            Задание: Вывести статистику посещений пользователем любых страниц сайта в течении суток. <br>
            Выполнил: Кононов Александр. Группа КИТ-20б <br> <br>
        </div>
        <?php
            echo "<p>" . $client . " загрузил <b>" . $_COOKIE[$client] . "</b> страниц сайта</p>";
        ?>
        <a href="index.php">главная страница</a>
     </body>
</html>