// ���������� ����: ���� | ������: ������� ������������� ������ (���. ���. "���.����.� ����." ���"���")
// �������: ������� ����������� ����������� ���������� ���������� GDI+ :
// ��������� ������ �� ����� C# ��� ����� NetFramework v.2.0 (���������� ����������� MSVS2005)
// =====================================================================================================================
// ������ ���������� �������:
// =====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;          //������ � ������� GDI+
using System.Drawing.Drawing2D;//�������� ��� ���������� ������� � GDI+
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
            // Form1_Paint() - �����-���������� ��������� WM_PAINT
            // sender - ������ �� �������� ���������: ���� ���� Form1
            // e - ������ �� �������� ������ Graphics ( == � Graphics ������� GDI+ )  

            // ��������� � ��� ������� (�����) Form1_Paint() ���������� ������� !!!
            //1 �������� ���������� GDIp - ������� � ��������� GDI+ �� ���� Graphics (������������ ��� �� ��������� � GDI+ !!!)  
            Graphics GDIp = e.Graphics;
            //2 ����������� ��� ������� ����� ���� ������� ����� ������
            GDIp.Clear(Color.White);
            //3 ����� ������ x=0,y=0
            GDIp.DrawString("����� ������� ���������� GDI+ :", new Font("Helvetica", 27), Brushes.Red, 0, 0);
            //4 ��������� ������ �����(X1,Y1 ������ ���)X1  Y1   X2  Y2 (����� �����: X1,Y1,X2,Y2)
            GDIp.DrawLine(new Pen(Color.Magenta), 10, 50, 550, 50);               // 4.1
            GDIp.DrawLine(new Pen(Brushes.Green, 2), 10, 60, 550, 60);             // 4.2 
            GDIp.DrawLine(new Pen(Brushes.Blue, 4), 550, 70, 10, 70);//X1 > X2 !!! // 4.3 
            Pen myPen = new Pen(Color.Black, 3);
            //!!!��� ����������� ����������������� ���������� (� ���� ���������� ����)
            //!!!���� ������� � ������ ����� ����� namespace: using System.Drawing.Drawing2D;
            myPen.DashStyle = DashStyle.Dash;//����� ��������� �����: - - - - - - - - -  
            GDIp.DrawLine(myPen, 550, 80, 10, 80);//�����: - - - - - - - - -       // 4.4
            myPen.DashStyle = DashStyle.DashDot;//�����: _._._._._._._._. 
            GDIp.DrawLine(myPen, 550, 90, 10, 90);//�����: _._._._._._._._.        // 4.5 
            myPen.DashStyle = DashStyle.DashDotDot; //�����:_.._.._.._.._.._.._..
            GDIp.DrawLine(myPen, 550, 100, 10, 100);//�����: _.._.._.._.._.._.._.. // 4.6
            myPen.Width = 7; myPen.DashStyle = DashStyle.Solid;//����� ��������� �����: �����������, �������� = 7 pix
            // ������������ ����������� ������ �����  
            myPen.StartCap = LineCap.ArrowAnchor;//������ � ������ �����
            //myPen.EndCap = LineCap.DiamondAnchor;//���� � ����� �����
            myPen.EndCap = LineCap.RoundAnchor;  //���������� � �����  �����
            GDIp.DrawLine(myPen, 10, 110, 546, 110);// == �����-������             // 4.7 
            //5 ��������� ����� (������ ��������� ���������� ����� � ����� ����� �������)
            GDIp.DrawLine(new Pen(Color.Black, 3), 10, 118, 550, 129);//��������   // 5.1
            GDIp.SmoothingMode = SmoothingMode.HighQuality; // ����� ����������� ���������� �����
            GDIp.DrawLine(new Pen(Color.Black, 3), 10, 128, 550, 139);//������ !!! // 5.2
            //6 ��������� ���������������               X1  Y1   dX   dY  
            GDIp.DrawRectangle(new Pen(Color.Green, 5), 10, 150, 250, 30);//�������                                                     // 6.1
            LinearGradientBrush gradBrush = new LinearGradientBrush(new Rectangle(0, 0, 280, 30), Color.Red, Color.Yellow, LinearGradientMode.Horizontal);
            GDIp.FillRectangle(gradBrush, 300, 150, 250, 30);//� ����������� ��������                                                   // 6.2 
            LinearGradientBrush gradBrush2 = new LinearGradientBrush(new Rectangle(0, 0, 50, 30), Color.Red, Color.Yellow, LinearGradientMode.Horizontal);
            GDIp.FillRectangle(gradBrush2, 300, 190, 250, 30);//� ����������� ��������                                                  // 6.3 
            GDIp.FillRectangle(new HatchBrush(HatchStyle.HorizontalBrick, Color.Yellow, Color.Red), 10, 190, 250, 30);//�������=������� // 6.4
            GDIp.FillRectangle(new HatchBrush(HatchStyle.Wave, Color.Yellow, Color.Red), 10, 230, 250, 30);//�������=���������� ������� // 6.5
            //7 �������-�������� ��������� ������ (��������� ����� ������� Point)          X    Y
            Point[] myPoints = { new Point(312, 230), new Point(340, 260), new Point(370, 238), new Point(400, 260), new Point(430, 238), new Point(460, 260), new Point(490, 230) };
            GDIp.DrawPolygon(new Pen(Color.Red, 6), myPoints);// ������������� �������� ����� ������ � �����
            // ������� � ���������� ���������� (>2) ������ 
            Point[] myPoints1 = { new Point(580, 210), new Point(580, 260), new Point(480, 260) };//����������� �������� �������
            PathGradientBrush pgradBrush = new PathGradientBrush(myPoints1);// ����� ��� ��������� ������������
            pgradBrush.SurroundColors = new Color[] { Color.Lime, Color.Yellow, Color.Cyan};//����� ��� ����� ������������ 
            GDIp.FillRectangle(pgradBrush, 480, 210, 100, 50);//������������� � ������� ������������� ������
             //8 ������                              X1  Y1   dX  dY
            GDIp.DrawEllipse(new Pen(Color.Red, 5), 10, 267, 80, 30);      //Draw... = ��� �������
            GDIp.FillEllipse(new SolidBrush(Color.Blue), 95, 267, 80, 30); //Fill... = � ��������
            //9 ������� �������                     X1  Y1  dX dY ��1 ��2
            GDIp.DrawArc(new Pen(Color.Red, 5), 180, 267, 80, 30, 0, 270);//0 - ������� �� ������������ ��� � �� ������ ��������(� "+" = ������ ������� ���.) , 270 - �������, ���������� ���� ������ ��������
            //10 ��������� ������� �������                 X1  Y1   dX  dY ��1 ��2
            GDIp.FillPie(new SolidBrush(Color.Blue), 265, 267, 80, 30, 0, 270);//0, 270 - ������� ... (��. ���������� ����������)
            // ������ (�������) � ������ �������� ������������ == ������ alpha (0-255) � ������� ����� �������� �����
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(255, 0,0,255)),    350, 267, 80, 30); //Fill... = � ��������
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(160, 255,0,0)),    400, 267, 80, 30); //Fill... = � ��������
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(200, Color.Green)),450, 267, 80, 30); //Fill... = � ��������
            GDIp.FillEllipse(new SolidBrush(Color.FromArgb(255, Color.Red)),  500, 267, 80, 30); //Fill... = � ��������
            //11 ������ �� �������� ������� �������� ������� (������ ����� � ������������ ��������)
            //11.1 ������ ����� (������ �� ��������: ���������� ������� = �������� �������� �������)� �������� ����������� ����������� � ������ ������� �������
            Point[] myPoints2 ={new Point(10,320), //myPoints2 = ������ �������� X,Y ��� 3-� �������� 
                                new Point(10,340),new Point(40,340),
                                                  new Point(40,320), 
                                                  new Point(40,340),new Point(70, 340), 
                                                                    new Point(70, 320),
                                                                    new Point(70, 300),new Point(100, 300),
                                                                                       new Point(100, 320)};
            GDIp.DrawBeziers(new Pen(Color.Red, 1), myPoints2);// ������ �����
            // ��������� ������ ����� (������ �������������) ��������� � �������� ����� ���������
            Point[] myPoints3 ={new Point(110,320), //myPoints3 = ������ �������� X,Y ��� 4-� �������� � ����������� ��������� ������ � ����� ������
                                new Point(110,300),new Point(140,300),
                                                   new Point(155,320), 
                                                   new Point(170,340),new Point(200, 340), 
                                                                      new Point(200, 320),
                                                                      new Point(200, 300),new Point(170, 300),
                                                                                          new Point(155, 320),
                                                                                          new Point(140, 340),new Point(110, 340),
                                                                                                              new Point(110, 320)};//��������� � ���������
            GDIp.DrawBeziers(new Pen(Color.Red, 3), myPoints3);// ������ ����� = �������������
            //11.2 ������ �� ���� ������������ (�����������, �������) ���������� �������� (��� ������� �����������)
            // ��������� ���� ����� == ������ ������ (60�40)
            Point[] myPoints4 ={ new Point(210, 310), new Point(270, 310), new Point(210, 330), new Point(240, 300), new Point(270, 330) }; //������ �������� X,Y ��� 5-�� ����� ������
            GDIp.DrawCurve(new Pen(Color.Red, 4), myPoints4, 0,4, 0.0f);//0.0f = ��������� ������
            Point[] myPoints5 ={ new Point(280, 310), new Point(340, 310), new Point(280, 330), new Point(310, 300), new Point(340, 330) }; //������ �������� X,Y ��� 5-�� ����� ������
            GDIp.DrawCurve(new Pen(Color.Red, 4), myPoints5, 0, 4, 0.7f);//0.7f = ��������� ������ 
            // ������ �������������� ���������� ������� ����� ������ ��� �������
            Point[] myPoints6 ={ new Point(350, 310), new Point(410, 310), new Point(350, 330), new Point(380, 300), new Point(410, 330) }; //������ �������� X,Y ��� 5-�� ����� ������
            GDIp.DrawClosedCurve(new Pen(Color.Red, 4), myPoints6, 0.7f, FillMode.Alternate);//0.7f = ��������� ������ 
            // ������ ������� (Fill...)
            Point[] myPoints7 ={ new Point(420, 310), new Point(480, 310), new Point(420, 330), new Point(450, 300), new Point(480, 330) }; //������ �������� X,Y ��� 5-�� ����� ������
            GDIp.FillClosedCurve(new SolidBrush(Color.Red), myPoints7, FillMode.Alternate, 0.0f);//0.0f = ��������� ������                                                             
            Point[] myPoints8 ={ new Point(490, 310), new Point(550, 310), new Point(490, 330), new Point(520, 300), new Point(550, 330) }; //������ �������� X,Y ��� 5-�� ����� ������
            GDIp.FillClosedCurve(new SolidBrush(Color.Red), myPoints8, FillMode.Winding, 0.0f);//0.0f = ��������� ������                                                             
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}    }
}