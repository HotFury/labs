<?php
    header("Content-Type: text/html; charset=utf-8");
?>
<html>
    <head>
        <title>lab#13</title>
    </head>
    <body>      
        <?php
            require './phpmailer/class.phpmailer.php'; 
            $reciver = $_POST['mail'];
            $mail = new PHPMailer(); 
            $message = "
            <html>
                <head>
                    <title>test mail (lab#13)</title>
                </head>
                <body>
                    <p>Hello, $reciver! </p>
                </body>
            </html>";
            $subject = 'lab #13';
            
            $uploaddir = './files/';
            $uploadfile = $uploaddir.basename($_FILES['file']['name']);
            copy($_FILES['file']['tmp_name'], $uploadfile);
            $file = $uploaddir . $_FILES['file']['name'];
            error_reporting(E_ALL ^ E_DEPRECATED);
            $mail->From = 'Kononov@lex'; 
            $mail->AddAddress($reciver, "Students"); 
            $mail->Subject = $subject;
            $mail->Body = $message; 
            $mail->IsHTML(true);
            $mail->AddAttachment($file,$file); 
            if ($mail->Send())
            {
                echo "email отправлен успешно!";
            }
        ?>
     </body>
</html>

