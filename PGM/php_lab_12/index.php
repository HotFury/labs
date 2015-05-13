<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#12</title>
    </head>
    <body>      
        Лабораторная работа №12 <br>
        10.05.2015 <br>
        Тема: функции PHP для работы с базами данных. <br>
        <div style="float:left; height: 20px">Задание:</div> 
        <div>а)  Клиенты,  имеющие  долги  по  услуге  с  наибольшим  количеством должников. <br>
             б)  клиенты  использующие  одновременно  услуги  исходящие международные звонки и входящие международные звонки.</div>
        <div>Выполнил: Кононов Александр. Группа КИТ-20б </div> <br>
        <br>       
        <form action="createNewBase.php" method="post">
            <div style="height: 25px; width: 450px"> Название новой базы<input style="float: right" type="text" name="dbName"></div>
            <div style="height: 25px; width: 450px">Название таблицы должников<input style="float: right" type="text" name="table1Name"></div>
            <div style="height: 25px; width: 450px">Название таблицы одинаковых тарифов<input style="float: right" type="text" name="table2Name"></div>
            <input type="submit" value="Применить">
        </form>
     </body>
</html>