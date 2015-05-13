<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#14</title>
    </head>
    <body>      
        Лабораторная работа №14 <br>
        11.05.2015 <br>
        Тема: Работа с графикой на РНР. <br>
        <div>Задание: Создать  график  из  табличных  данных  с  количеством  значений  не менее 30.</div>
        <div>Выполнил: Кононов Александр. Группа КИТ-20б </div> <br>
        <br>    
        <div style='float: left; width: 50px; text-align: center;'>X</div>
        <div style='float: left; text-align: center; width: 50px;'>Y</div>
        <div>|</div>
        <form action="createPlot.php" method="post">
            <?php
                for ($i = 0; $i < 30; $i++)
                {
                    $val = $i * $i;
                    echo "<div style='float:left;'><input style='width:50px' value = $val type='text' name='x$i'></div>";
                    echo "<div ><input style='width:50px' type='text' value = $i name='y$i'></div>";
                }
            ?>
            <input type="submit" value="Отправить">
        </form>
     </body>
</html>