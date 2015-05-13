<?php
    header("Content-Type: text/html; charset=utf-8");
    require_once('domxml-php4-to-php5.php');
?>
<html>
    <head>
        <title>lab#15</title>
    </head>
    <body>      
        Лабораторная работа №15 <br>
        11.05.2015 <br>
        Тема: Использование XML в РНР. <br>
        <div>Задание: Создать XML-файл на тему "Мобильные телефоны", обязательные поля: "Фирма", "Модель", "Количество", "Цена".</div>
        <div>Выполнил: Кононов Александр. Группа КИТ-20б </div> <br>
        <br>       
        <?php
            include 'xml.php';
            $xml = new SimpleXMLElement($xmlStr);
            echo "Файл xml <br>"; 
            echo '<table border="1">';
            echo '<thead>';
            echo '<tr>';
            echo '<th>Фирма</th>';
            echo '<th>Модель</th>';
            echo '<th>Количество</th>';
            echo '<th>Цена</th>';
            echo '</tr>';
            echo '</thead>';
            echo '<tbody>';
            
            foreach ($xml->phone as $phone)
            {
                echo '<tr>';
                echo '<td>' . $phone->firm."<br>" . '</td>';
                echo '<td>' . $phone->model."<br>" . '</td>';
                echo '<td>' . $phone->count."<br>" . '</td>';
                echo '<td>' . $phone->prise."<br>" . '</td>';
            }
            echo '</tbody>';
            echo '</table>';
        ?>
     </body>
</html>