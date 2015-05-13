<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#12</title>
    </head>
    <body> 
    <?php
        $dataBase = 'services_payment';
        $dbName = $_POST['dbName'];
        $table1Name = $_POST['table1Name'];
        $table2Name = $_POST['table2Name'];
        $hostname = "localhost"; 
        $username = "forScript"; 
        $password = "123654789";
        $db = mysql_connect($hostname, $username, $password) 
            or die ("Не могу создать соединение");
        
        mysql_select_db($dataBase,$db) or die("Ошибка выбора БД"); 
        $sql = 'select * from Clients left outer join Services on clients.tariff_plan = title right outer join debts on clients.bill = debts.bill'; 
        $result = mysql_query($sql, $db);
        if (!$result)
        { 
            echo "Ошибка запроса: ".mysql_error()."<br />"; 
        }
        while ($row = mysql_fetch_row($result)) 
        { 
            $bills[] = $row[0];
            $names[] = $row[4];
            $tariff[] = $row[8];
            $debts[] = $row[13];
            $amount[] = $row[14];
        } 
        $repeats = array_count_values($debts);
        $maxRepeat = 0;
        foreach($repeats as $key => $value)
        {
            if ($value > $maxRepeat)
            {
                $maxRepeat = $value;
                $maxDebtCount = $key;
            }
        }
        
        $sql = 'CREATE DATABASE '.$dbName; 
        if (mysql_query($sql, $db)) 
        { 
            echo "БД $dbName создана<br>"; 
        } 
        else 
        { 
            echo "Ошибка создания БД: ".mysql_error()."<br>"; 
        }
        mysql_select_db($dbName,$db) or die("Ошибка выбора БД"); 
        $sql = 'CREATE TABLE ' .$table1Name. ' (clients_bill int, client_name char(50), service_title char(50), debt double, primary key (clients_bill) )'; 
        if (mysql_query($sql, $db))
        { 
            echo "Таблица $table1Name создана<br />"; 
        } 
        else
        { 
            echo "Ошибка создания таблицы: ".mysql_error()."<br />"; 
        }
        
        for ($i = 0; $i < sizeof($debts); $i++)
        {
            if ($debts[$i] == $maxDebtCount)
            {
                $sql = "insert into $table1Name values ($bills[$i], '$names[$i]', '$tariff[$i]', $debts[$i])";
                if (!mysql_query($sql, $db))
                { 
                    echo "Ошибка запроса: ".mysql_error()."<br />"; 
                }
                
            }
        }
    
        $sql = 'CREATE TABLE ' .$table2Name. ' (clients_bill int, client_name char(50), adress char(100), region char(50), primary key (clients_bill) )'; 
        if (mysql_query($sql, $db))
        { 
            echo "Таблица $table2Name создана<br />"; 
        } 
        else
        { 
            echo "Ошибка создания таблицы: ".mysql_error()."<br />"; 
        }
        mysql_select_db($dataBase,$db) or die("Ошибка выбора БД"); 
        $sql1 = "select * from Clients left outer join Services on clients.tariff_plan = title where category = 'исходящие международные звонки' or category = 'входящие международные звонки'";
        $result1 = mysql_query($sql1, $db);
        if (!$result1)
        { 
            echo "Ошибка запроса: ".mysql_error()."<br />"; 
        }
        while ($row = mysql_fetch_row($result1)) 
        {
            $bills2[] = $row[0];
            $names2[] = $row[4];
            $adres2[] = $row[5];
            $region2[] = $row[6];
        }
        $repeats1 = array_count_values($names2);
        $maxRepeat1 = 2;
        foreach($repeats1 as $key => $value)
        {
            if ($value >= $maxRepeat1)
            {
                $namesOut[] = $key;
            }
        }
        mysql_select_db($dbName,$db) or die("Ошибка выбора БД"); 
        for ($i = 0; $i < sizeof($names2); $i++)
        {
            foreach ($namesOut as $val)
            {
                if ($names2[$i] == $val)
                {
                    $sql = "insert into $table2Name values ($bills2[$i], '$names2[$i]', '$adres2[$i]', '$region2[$i]')";
                    if (!mysql_query($sql, $db))
                    { 
                        echo "Ошибка запроса: ".mysql_error()."<br />"; 
                    }
                }
            }
        }
        
        echo "Таблица $table1Name <br>"; 
        echo '<table border="1">';
        echo '<thead>';
        echo '<tr>';
        echo '<th>clients_bill</th>';
        echo '<th>client_name</th>';
        echo '<th>service_title</th>';
        echo '<th>debt</th>';
        echo '</tr>';
        echo '</thead>';
        echo '<tbody>';
        $qr_result = mysql_query("select * from " . $table1Name)
            or die(mysql_error());
        while($data = mysql_fetch_array($qr_result))
        { 
            echo '<tr>';
            echo '<td>' . $data['clients_bill'] . '</td>';
            echo '<td>' . $data['client_name'] . '</td>';
            echo '<td>' . $data['service_title'] . '</td>';
            echo '<td>' . $data['debt'] . '</td>';
            echo '</tr>';
        }
        echo '</tbody>';
        echo '</table>';
        
        
        echo "Таблица $table2Name <br>"; 
        echo '<table border="1">';
        echo '<thead>';
        echo '<tr>';
        echo '<th>clients_bill</th>';
        echo '<th>client_name</th>';
        echo '<th>adress</th>';
        echo '<th>region</th>';
        echo '</tr>';
        echo '</thead>';
        echo '<tbody>';
        $qr_result = mysql_query("select * from " . $table2Name)
            or die(mysql_error());
        while($data = mysql_fetch_array($qr_result))
        { 
            echo '<tr>';
            echo '<td>' . $data['clients_bill'] . '</td>';
            echo '<td>' . $data['client_name'] . '</td>';
            echo '<td>' . $data['adress'] . '</td>';
            echo '<td>' . $data['region'] . '</td>';
            echo '</tr>';
        }
        echo '</tbody>';
        echo '</table>';
        
        
        
    ?>
     </body>
</html>