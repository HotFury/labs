// Лекционный курс: ППКК | Лектор: Евгений Александрович Лобода (доц. каф. "Выч.техн.и прог." НТУ"ХПИ")
// ЗАДАНИЕ: вывести изображение графических примитивов средствами GDI+ :
// Программу писать на языке C# для среды NetFramework v.2.0 (средствами компилятора MSVS2005)
// =====================================================================================================================
// Пример выполнения задания:
// =====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;          //Доступ к методам GDI+
using System.Drawing.Drawing2D;//Добавить для расширения доступа к GDI+
using System.Text;
using System.Windows.Forms;

namespace MyGDIp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Form1_Paint() - метод-обработчик сообщений WM_PAINT
            // sender - ссылка на источник сообщения: наше окно Form1
            // e - ссылка на родителя класса Graphics ( == в Graphics функции GDI+ )  

            // Операторы в эту функцию (метод) Form1_Paint() записывает студент !!!
            //1 Создание экземпляра GDIp - объекта с функциями GDI+ на базе Graphics (обязательный шаг до обращения к GDI+ !!!)  
            Graphics GDIp = e.Graphics;
            //2 Закрашиваем всю рабочую часть окна проекта белым цветом
            GDIp.Clear(Color.White);
            //3 Вывод текста x=0,y=0
            GDIp.DrawString("Вывод графики средствами GDI+ :", new Font("Helvetica", 27), Brushes.Red, 0, 0);
            //4 Рисование прямых линий(X1,Y1 начало лин)X1  Y1   X2  Y2 (линия между: X1,Y1,X2,Y2)
            GDIp.DrawLine(new Pen(Color.Magenta), 10, 50, 550, 50);               // 4.1
            GDIp.DrawLine(new Pen(Brushes.Green, 2), 10, 60, 550, 60);             // 4.2 
            GDIp.DrawLine(new Pen(Brushes.Blue, 4), 550, 70, 10, 70);//X1 > X2 !!! // 4.3 
            Pen myPen = new Pen(Color.Black, 3);
            //!!!Для обеспечения работоспособности следующего (и ряда операторов ниже)
            //!!!надо вписать в начале этого файла namespace: using System.Drawing.Drawing2D;
            myPen.DashStyle = DashStyle.Dash;//стиль рисования линии: - - - - - - - - -  
            GDIp.DrawLine(myPen, 550, 80, 10, 80);//линия: - - - - - - - - -       // 4.4
            myPen.DashStyle = DashStyle.DashDot;//стиль: _._._._._._._._. 
            GDIp.DrawLine(myPen, 550, 90, 10, 90);//линия: _._._._._._._._.        // 4.5 
            myPen.DashStyle = DashStyle.DashDotDot; //стиль:_.._.._.._.._.._.._..
            GDIp.DrawLine(myPen, 550, 100, 10, 100);//линия: _.._.._.._.._.._.._.. // 4.6
            myPen.Width = 7; myPen.DashStyle = DashStyle.Solid;//стиль рисования линии: непрерывная, толщиной = 7 pix
            // Формирование изображения концов линии  
            myPen.StartCap = LineCap.ArrowAnchor;//стрела в начале линии
            //myPen.EndCap = LineCap.DiamondAnchor;//ромб в конце линии
            myPen.EndCap = LineCap.RoundAnchor;  //окружность в конце  линии
            GDIp.DrawLine(myPen, 10, 110, 546, 110);// == линия-стрела             // 4.7 
            //5 Наклонные линии (удалим появления неровности линий с малым углом наклона)
            GDIp.DrawLine(new Pen(Color.Black, 3), 10, 118, 550, 129);//неровная   // 5.1
            GDIp.SmoothingMode = SmoothingMode.HighQuality; // режим сглаживания неровности линий
            GDIp.DrawLine(new Pen(Color.Black, 3), 10, 128, 550, 139);//ровная !!! // 5.2
            //6 Рисование прямоугольников               X1  Y1   dX   dY  
            GDIp.DrawRectangle(new Pen(Color.Green, 5), 10, 150, 250, 30);//обычный                                                     // 6.1
            LinearGradientBrush gradBrush = new LinearGradientBrush(new Rectangle(0, 0, 280, 30), Color.Red, Color.Yellow, LinearGradientMode.Horizontal);
            GDIp.FillRectangle(gradBrush, 300, 150, 250, 30);//с градиентной заливкой                                                   // 6.2 
            LinearGradientBrush gradBrush2 = new LinearGradientBrush(new Rectangle(0, 0, 50, 30), Color.Red, Color.Yellow, LinearGradientMode.Horizontal);
            GDIp.FillRectangle(gradBrush2, 300, 190, 250, 30);//с градиентной заливкой                                                  // 6.3 
            GDIp.FillRectangle(new HatchBrush(HatchStyle.HorizontalBrick, Color.Yellow, Color.Red), 10, 190, 250, 30);//заливка=кирпичи // 6.4
            GDIp.FillRectangle(new HatchBrush(HatchStyle.Wave, Color.Yellow, Color.Red), 10, 230, 250, 30);//заливка=волнистыми линиями // 6.5
            //7 Кусочно-линейная замкнутая кривая (соединяет точки массива Point)          X    Y
            Point[] myPoints = { new Point(312, 230), new Point(340, 260), new Point(370, 238), new Point(400, 260), new Point(430, 238), new Point(460, 260), new Point(490, 230) };
            GDIp.DrawPolygon(new Pen(Color.Red, 6), myPoints);// автоматически замкнула точки начала и конца
            // Заливка с градиентом нескольких (>2) цветов 
            Point[] myPoints1 = { new Point(580, 210), new Point(580, 260), new Point(480, 260) };//треугольник действия заливки
            PathGradientBrush pgradBrush = new PathGradientBrush(myPoints1);// кисть для заданного треугольника
            pgradBrush.SurroundColors = new Color[] { Color.Lime, Color.Yellow, Color.Cyan};//цвета для углов треугольника 
            GDIp.FillRectangle(pgradBrush, 480, 210, 100, 50);//прямоугольник с залитым треугольником внутри
             //8 Эллипс                              X1  Y1   dX  dY
            GDIp.DrawEllipse(new Pen(Color.Red, 5), 10, 267, 80, 30);      //Draw... = без заливки
            GDIp.FillEllipse(new SolidBrush(Color.Blue), 95, 267, 80, 30); //Fill... = с заливкой
            //9 Сегмент эллипса                     X1  Y1  dX dY Гр1 Гр2
            GDIp.DrawArc(new Pen(Color.Red, 5), 180, 267, 80, 30, 0, 270);//0 - градусы от вертикальной оси Х до начала сегмента(с "+" = против часовой стр.) , 270 - градусы, занимаемые всей кривой сегмента
            //10 Замкнутый сегмент эллипса                 X1  Y1   dX  dY Гр1 Гр2
            GDIp.FillPie(new SolidBrush(Color.Blue), 265, 267, 80, 30, 0, 270);//0, 270 - градусы ... (см. предыдущий коментарий)
            // Фигуры (эллипсы) с разной степенью прозрачности == задаем alpha (0-255) в старших битах значении цвета
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(255, 0,0,255)),    350, 267, 80, 30); //Fill... = с заливкой
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(160, 255,0,0)),    400, 267, 80, 30); //Fill... = с заливкой
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(200, Color.Green)),450, 267, 80, 30); //Fill... = с заливкой
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(255, Color.Red)),  500, 267, 80, 30); //Fill... = с заливкой
            //11 Кривые из отрезков парабол третьего порядка (кривые Безье и классических сплайнов)
            //11.1 Кривые Безье (кривые из отрезков: кубические сплайны = полиномы третьего порядка)с заданием направления касательных в концах каждого сплайна
            Point[] myPoints2 ={new Point(10,320), //myPoints2 = массив значений X,Y для 3-х сплайнов 
                                new Point(10,340),new Point(40,340),
                                                  new Point(40,320), 
                                                  new Point(40,340),new Point(70, 340), 
                                                                    new Point(70, 320),
                                                                    new Point(70, 300),new Point(100, 300),
                                                                                       new Point(100, 320)};
            GDIp.DrawBeziers(new Pen(Color.Red, 1), myPoints2);// кривая Безье
            // Замкнутая кривая Безье (сиивол бесконечности) начальная и конечная точки совпадают
            Point[] myPoints3 ={new Point(110,320), //myPoints3 = массив значений X,Y для 4-х сплайнов с совпадением координат начала и конца кривой
                                new Point(110,300),new Point(140,300),
                                                   new Point(155,320), 
                                                   new Point(170,340),new Point(200, 340), 
                                                                      new Point(200, 320),
                                                                      new Point(200, 300),new Point(170, 300),
                                                                                          new Point(155, 320),
                                                                                          new Point(140, 340),new Point(110, 340),
                                                                                                              new Point(110, 320)};//совпадает с начальной
            GDIp.DrawBeziers(new Pen(Color.Red, 3), myPoints3);// кривая Безье = бесконечность
            //11.2 Кривые на базе канонических (стандартных, обычных) кубических сплайнов (без задания касательных)
            // Соединяем пять точек == аналог звезды (60ч40)
            Point[] myPoints4 ={ new Point(210, 310), new Point(270, 310), new Point(210, 330), new Point(240, 300), new Point(270, 330) }; //массив значений X,Y для 5-ти точек кривой
            GDIp.DrawCurve(new Pen(Color.Red, 4), myPoints4, 0,4, 0.0f);//0.0f = жесткость кривой
            Point[] myPoints5 ={ new Point(280, 310), new Point(340, 310), new Point(280, 330), new Point(310, 300), new Point(340, 330) }; //массив значений X,Y для 5-ти точек кривой
            GDIp.DrawCurve(new Pen(Color.Red, 4), myPoints5, 0, 4, 0.7f);//0.7f = жесткость кривой 
            // Вводим автоматическое соединение крайних точек кривой без заливки
            Point[] myPoints6 ={ new Point(350, 310), new Point(410, 310), new Point(350, 330), new Point(380, 300), new Point(410, 330) }; //массив значений X,Y для 5-ти точек кривой
            GDIp.DrawClosedCurve(new Pen(Color.Red, 4), myPoints6, 0.7f, FillMode.Alternate);//0.7f = жесткость кривой 
            // Вводим заливку (Fill...)
            Point[] myPoints7 ={ new Point(420, 310), new Point(480, 310), new Point(420, 330), new Point(450, 300), new Point(480, 330) }; //массив значений X,Y для 5-ти точек кривой
            GDIp.FillClosedCurve(new SolidBrush(Color.Red), myPoints7, FillMode.Alternate, 0.0f);//0.0f = жесткость кривой                                                             
            Point[] myPoints8 ={ new Point(490, 310), new Point(550, 310), new Point(490, 330), new Point(520, 300), new Point(550, 330) }; //массив значений X,Y для 5-ти точек кривой
            GDIp.FillClosedCurve(new SolidBrush(Color.Red), myPoints8, FillMode.Winding, 0.0f);//0.0f = жесткость кривой                                                             
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}    }
}