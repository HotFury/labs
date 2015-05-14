using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
namespace nk_lab_5
{
    public static class Constants
    {
        public const int vectorsCount = 9;
        public static int axonCount = 3;
        public static int sensorCount;

    }
    partial class Sensor
    {
        private int input;
        /*public int Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }*/
        public Sensor(int inp)
        {
            input = inp;
        }
    }
    partial class Axon
    {
        private int[] weight;
        private decimal distance;
        public Axon(int sensorCount)
        {
            weight = new int[sensorCount];
        }
    }
    partial class KohonenNet
    {
        private Sensor[] sensor;
        private Axon[] axon;
        public KohonenNet(int sensorCount)
        {
            sensor = new Sensor[sensorCount];
            axon = new Axon[Constants.axonCount];
            for (int i = 0; i < axon.Length; i++)
            {
                axon[i] = new Axon(sensorCount);
            }
        }
        public void InitSensor(List<int> vector)
        {
            for (int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = new Sensor(vector[i]);
            }
        }
    }
    partial class Log
    {
        public void WriteToLogString(string str)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            write_text.WriteLine(str);
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLogHead(decimal[] arr, string col, int length)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            string bound = "";
            for (int i = 0; i < length; i++)
                bound += "=";
            bound += "|";
            string bounder = bound;
            string head = String.Format("{0," + length.ToString() + ":0.000}", "") + "|";
            for (int i = 0; i < arr.Length; i++)
            {
                bounder += bound;
                head += String.Format("{0," + length.ToString() + ":0.000}", col + i.ToString()) + "|";
            }
            write_text.WriteLine(bounder);
            write_text.WriteLine(head);
            write_text.WriteLine(bounder);
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLogHead(List<int> arr, string col, int length)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            string bound = "";
            for (int i = 0; i < length; i++)
                bound += "=";
            bound += "|";
            string bounder = bound;
            string head = String.Format("{0," + length.ToString() + ":0.000}", "") + "|";
            for (int i = 0; i < arr.Count; i++)
            {
                bounder += bound;
                head += String.Format("{0," + length.ToString() + ":0.000}", col + i.ToString()) + "|";
            }
            write_text.WriteLine(bounder);
            write_text.WriteLine(head);
            write_text.WriteLine(bounder);
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLog(decimal[] arr, string row, int num)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            /*string inString = ArrayToString(arr, "|");
            string[] values = inString.Split('|');*/
            string outString = String.Format("{0,5:0.###}", row + num.ToString()) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            foreach (decimal val in arr)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.###}", val) + "|";
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLog(int[] arr, string row, int num)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            /*string inString = ArrayToString(arr, "|");
            string[] values = inString.Split('|');*/
            string outString = String.Format("{0,5:0.###}", row + num.ToString()) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            foreach (decimal val in arr)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.###}", val) + "|";
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLog(List<int> arr, string row, int num)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            /*string inString = ArrayToString(arr, "|");
            string[] values = inString.Split('|');*/
            string outString = String.Format("{0,5:0.###}", row + num.ToString()) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            foreach (decimal val in arr)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.###}", val) + "|";
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }
        /*public void WriteToLog(A_neuron[] values, string row, int num, string whatOut)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            //string inString = ArrayToString(arr, "|");
            //string[] values = inString.Split('|');
            string outString = "";
            if (num != -1)
                outString = String.Format("{0,5:0.000}", row + num.ToString()) + "|";
            else
                outString = String.Format("{0,5:0.000}", row) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            if (whatOut == "threshold")
            {
                foreach (A_neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.0}", val.Threshold) + "|";
                }
            }
            else if (whatOut == "outputDec")
            {
                foreach (A_neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.0}", val.OutputDec) + "|";
                }
            }
            else if (whatOut == "output")
            {
                foreach (A_neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.0}", val.Output) + "|";
                }
            }
            else if (whatOut == "input")
            {
                foreach (A_neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.0}", val.Input) + "|";
                }
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }*/

        public string ArrayToString(decimal[] array, string delimiter)
        {
            if (array != null)
            {
                // edit: replaced my previous implementation to use StringBuilder
                if (array.Length > 0)
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append(array[0]);
                    for (int i = 1; i < array.Length; i++)
                    {
                        builder.Append(delimiter);
                        builder.Append(array[i]);
                    }

                    return builder.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return null;
            }
        }
    }
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.firstName = new System.Windows.Forms.TextBox();
            this.middleName = new System.Windows.Forms.TextBox();
            this.surName = new System.Windows.Forms.TextBox();
            this.alphaLabel = new System.Windows.Forms.Label();
            this.Rlabel = new System.Windows.Forms.Label();
            this.alphaValue = new System.Windows.Forms.TextBox();
            this.rValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.kValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 56);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "First name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Middle name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Surname";
            // 
            // firstName
            // 
            this.firstName.Location = new System.Drawing.Point(124, 25);
            this.firstName.Name = "firstName";
            this.firstName.Size = new System.Drawing.Size(100, 20);
            this.firstName.TabIndex = 4;
            // 
            // middleName
            // 
            this.middleName.Location = new System.Drawing.Point(124, 64);
            this.middleName.Name = "middleName";
            this.middleName.Size = new System.Drawing.Size(100, 20);
            this.middleName.TabIndex = 5;
            // 
            // surName
            // 
            this.surName.Location = new System.Drawing.Point(124, 103);
            this.surName.Name = "surName";
            this.surName.Size = new System.Drawing.Size(100, 20);
            this.surName.TabIndex = 6;
            // 
            // alphaLabel
            // 
            this.alphaLabel.AutoSize = true;
            this.alphaLabel.Location = new System.Drawing.Point(13, 64);
            this.alphaLabel.Name = "alphaLabel";
            this.alphaLabel.Size = new System.Drawing.Size(26, 13);
            this.alphaLabel.TabIndex = 7;
            this.alphaLabel.Text = "α = ";
            // 
            // Rlabel
            // 
            this.Rlabel.AutoSize = true;
            this.Rlabel.Location = new System.Drawing.Point(13, 90);
            this.Rlabel.Name = "Rlabel";
            this.Rlabel.Size = new System.Drawing.Size(27, 13);
            this.Rlabel.TabIndex = 8;
            this.Rlabel.Text = "R = ";
            // 
            // alphaValue
            // 
            this.alphaValue.Location = new System.Drawing.Point(34, 61);
            this.alphaValue.Name = "alphaValue";
            this.alphaValue.Size = new System.Drawing.Size(32, 20);
            this.alphaValue.TabIndex = 9;
            // 
            // rValue
            // 
            this.rValue.Location = new System.Drawing.Point(34, 87);
            this.rValue.Name = "rValue";
            this.rValue.Size = new System.Drawing.Size(32, 20);
            this.rValue.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(325, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Check word";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(328, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            // 
            // teach
            // 
            this.teach.Location = new System.Drawing.Point(12, 139);
            this.teach.Name = "teach";
            this.teach.Size = new System.Drawing.Size(75, 23);
            this.teach.TabIndex = 13;
            this.teach.Text = "Create net";
            this.teach.UseVisualStyleBackColor = true;
            this.teach.Click += new System.EventHandler(this.teach_Click);
            // 
            // recognize
            // 
            this.recognize.Location = new System.Drawing.Point(328, 52);
            this.recognize.Name = "recognize";
            this.recognize.Size = new System.Drawing.Size(75, 23);
            this.recognize.TabIndex = 14;
            this.recognize.Text = "Recognize";
            this.recognize.UseVisualStyleBackColor = true;
            this.recognize.Click += new System.EventHandler(this.recognize_Click);
            // 
            // about
            // 
            this.about.Location = new System.Drawing.Point(12, 168);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(75, 23);
            this.about.TabIndex = 15;
            this.about.Text = "About";
            this.about.UseVisualStyleBackColor = true;
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // kValue
            // 
            this.kValue.Location = new System.Drawing.Point(34, 113);
            this.kValue.Name = "kValue";
            this.kValue.Size = new System.Drawing.Size(32, 20);
            this.kValue.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "k = ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 229);
            this.Controls.Add(this.kValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.about);
            this.Controls.Add(this.recognize);
            this.Controls.Add(this.teach);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rValue);
            this.Controls.Add(this.alphaValue);
            this.Controls.Add(this.Rlabel);
            this.Controls.Add(this.alphaLabel);
            this.Controls.Add(this.surName);
            this.Controls.Add(this.middleName);
            this.Controls.Add(this.firstName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox firstName;
        private System.Windows.Forms.TextBox middleName;
        private System.Windows.Forms.TextBox surName;
        private System.Windows.Forms.Label alphaLabel;
        private System.Windows.Forms.Label Rlabel;
        private System.Windows.Forms.TextBox alphaValue;
        private System.Windows.Forms.TextBox rValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button teach;
        private System.Windows.Forms.Button recognize;
        private System.Windows.Forms.Button about;
        private TextBox kValue;
        private Label label5;
    }
}

