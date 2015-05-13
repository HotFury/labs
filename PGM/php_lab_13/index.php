<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#13</title>
    </head>
    <body>      
        Лабораторная работа №13 <br>
        11.05.2015 <br>
        Тема: Формирование и отправка электронных писем с помощью РНР. <br>
        <div>Задание: Сформировать и отправить письмо в HTML формате с вложенными файлами на указанный email с помощью библиотеки РНРMailer.</div>
        <div>Выполнил: Кононов Александр. Группа КИТ-20б </div> <br>
        <br>       
        <form action="sendMail.php" enctype="multipart/form-data" method="post">
            <div style="height: 25px; width: 350px"> Выберете файл <input style="float: right" type="file" name="file"></div>
            <div style="height: 25px; width: 250px"> Ваш email: <input style="float: right" type="text" name="mail"></div>
            <input type="submit" value="Отправить">
        </form>
     </body>
</html>