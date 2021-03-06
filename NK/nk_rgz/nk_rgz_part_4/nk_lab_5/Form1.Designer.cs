﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
namespace nk_lab_5
{
    public static class Constants
    {
        public const int lettersCount = 5;
        public const int maxInputCount = 150;
        public static int axonCount = lettersCount;
        public static int sensorCount;
        public static decimal lim = 0.0m;

    }
    partial class Sensor
    {
        private int input;
        public int Input
        {
            get
            {
                return input;
            }
        }
        public Sensor(int inp)
        {
            input = inp;
        }
    }
    partial class Axon
    {
        private decimal[] weight;
        private decimal[] oldWeight;
        public decimal[] Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }
        public decimal[] OldWeight
        {
            get
            {
                return oldWeight;
            }
            set
            {
                oldWeight = value;
            }
        }
        
        private decimal distance;
        public decimal Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }
        public Axon(int sensorCount)
        {
            weight = new decimal[sensorCount];
            oldWeight = new decimal[sensorCount];
        }
        public void InitWeight(List<int> inp)
        {
            for (int i = 0; i < weight.Length; i++)
            {
                weight[i] = inp[i];
                oldWeight[i] = weight[i];
            }
        }
    }
    partial class KohonenNet : Log
    {
        private Sensor[] sensor;
        private Axon[] axon;
        private decimal alpha;
        private int radius;
        private decimal coefficient;
        private int iterations;
        public int Iteration
        {
            get
            {
                return iterations;
            }
        }

        public KohonenNet(int sensorCount, decimal alphaValue, int radiusValue, decimal coeff)
        {
            alpha = alphaValue;
            coefficient = coeff;
            radius = radiusValue;
            iterations = 0;
            sensor = new Sensor[sensorCount];
            axon = new Axon[Constants.axonCount];
            for (int i = 0; i < axon.Length; i++)
            {
                axon[i] = new Axon(sensor.Length);
            }
        }
        public void InitSensors(List<int> vector)
        {
            for (int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = new Sensor(vector[i]);
            }
        }
        public void InitAxons(List<List<int>> inp, ref System.Diagnostics.Stopwatch teachTime)
        {
            WriteToLogString(" ========== WEIGHTS ========== ");
            WriteToLogHead(axon[0].Weight, "w", 5);
            teachTime.Start();
            for(int i = 0; i < axon.Length; i++)
            {
                axon[i].InitWeight(inp[i]);
                teachTime.Stop();
                WriteToLog(axon[i].Weight, "A", i);
                teachTime.Start();
            }
            teachTime.Stop();
        }
        public void CalculateDistance(ref System.Diagnostics.Stopwatch recTime)
        {
            recTime.Start();
            for (int i = 0; i < axon.Length; i++)
            {
                axon[i].Distance = 0;
                for (int j = 0; j < axon[i].Weight.Length; j++)
                {
                    axon[i].Distance += (axon[i].Weight[j] - sensor[j].Input) * (axon[i].Weight[j] - sensor[j].Input);
                }
            }
            recTime.Stop();
            WriteToLog(axon, "Dist", -1, "distance");
        }
        public int GetWinnerAxon()
        {
            int minNum = 0;
            decimal min = axon[minNum].Distance;
            for (int i = 1; i < axon.Length; i++)
            {
                if (axon[i].Distance < min)
                {
                    min = axon[i].Distance;
                    minNum = i;
                }
            }
            return minNum;
        }
        public void CorrectWeights(int axonNumber)
        {
            int lowElNum = axonNumber - radius;
            int highElNum = axonNumber + radius;
            if (lowElNum < 0)
                lowElNum = 0;
            if (highElNum >= axon.Length)
                highElNum = axon.Length - 1;
            
            for (int j = lowElNum; j <= highElNum; j++)
            {
                for (int i = 0; i < axon[j].Weight.Length; i++)
                {
                    axon[j].OldWeight[i] = axon[j].Weight[i];
                    axon[j].Weight[i] = axon[j].Weight[i] + alpha * (sensor[i].Input - axon[j].Weight[i]);
                }
            }
        }
        public int Recognize(List<int> vector, ref System.Diagnostics.Stopwatch recTime)
        {
            InitSensors(vector);
            CalculateDistance(ref recTime);
            return GetWinnerAxon();
        }
        public bool Stop()
        {
            for (int i = 0; i < axon.Length; i++)
            {
                for (int j = 0; j < axon[i].Weight.Length; j++)
                {
                    if (Math.Abs(axon[i].Weight[j] - axon[i].OldWeight[j]) > Constants.lim)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool Teach(List<List<int>> vectors, ref System.Windows.Forms.ProgressBar progBar, ref System.Diagnostics.Stopwatch teachTime)
        {
            progBar.Maximum++;
            progBar.PerformStep();
            teachTime.Start();
            iterations++;
            foreach (List<int> curVector in vectors)
            {
                int axonNum = Recognize(curVector, ref teachTime);
                CorrectWeights(axonNum);
                teachTime.Stop();
                WriteToLogHead(axon[0].Weight, "w", 5);
                for (int i = 0; i < axon.Length; i++)
                {
                    WriteToLog(axon[i].Weight, "A", i);
                }
                teachTime.Start();
            }
            alpha = alpha * coefficient;
            if (radius > 0)
            {
                radius--;
            }
            if (!Stop())
            {
                Teach(vectors, ref progBar, ref teachTime);
            }
            return true;
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
        public void WriteToLog(Axon[] values, string row, int num, string whatOut)
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
            if (whatOut == "distance")
            {
                foreach (Axon val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.00}", val.Distance) + "|";
                }
            }
            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }

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



            this.heightValue = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.widthValue = new System.Windows.Forms.TextBox();
            this.kValue = new System.Windows.Forms.TextBox();
            this.k = new System.Windows.Forms.Label();
            this.r = new System.Windows.Forms.Label();
            this.rValue = new System.Windows.Forms.TextBox();
            this.alpha = new System.Windows.Forms.Label();
            this.alphaValue = new System.Windows.Forms.TextBox();
            this.showCheck = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.showCheck = new System.Windows.Forms.Button();
            this.signs = new System.Windows.Forms.Label[Constants.lettersCount];
            this.toRecSign = new System.Windows.Forms.Label();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                signs[i] = new System.Windows.Forms.Label();
                System.Windows.Forms.CheckBox[] letter = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
                for (int j = 0; j < Constants.maxInputCount; j++)
                {
                    letter[j] = new System.Windows.Forms.CheckBox();
                }
                this.letters.Add(letter);
            }
            this.standart = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.standart[i] = new System.Windows.Forms.CheckBox();
            }
            this.loading = new System.Windows.Forms.ProgressBar();
            this.loadLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();

            this.SuspendLayout();
            // 
            // heightValue
            // 
            this.heightValue.Location = new System.Drawing.Point(147, 3);
            this.heightValue.Name = "heightValue";
            this.heightValue.Size = new System.Drawing.Size(32, 20);
            this.heightValue.TabIndex = 1;
            // 
            // height
            // 
            this.height.AutoSize = true;
            this.height.Location = new System.Drawing.Point(106, 6);
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(36, 13);
            this.height.TabIndex = 3;
            this.height.Text = "height";
            // 
            // width
            // 
            this.width.AutoSize = true;
            this.width.Location = new System.Drawing.Point(195, 6);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(32, 13);
            this.width.TabIndex = 5;
            this.width.Text = "width";
            // 
            // widthValue
            // 
            this.widthValue.Location = new System.Drawing.Point(236, 3);
            this.widthValue.Name = "widthValue";
            this.widthValue.Size = new System.Drawing.Size(32, 20);
            this.widthValue.TabIndex = 4;

            // 
            // alphaValue
            // 
            this.alphaValue.Location = new System.Drawing.Point(135, 53);
            this.alphaValue.Name = "alphaValue";
            this.alphaValue.Size = new System.Drawing.Size(32, 20);
            this.alphaValue.TabIndex = 4;
            // 
            // alpha
            // 
            this.alpha.AutoSize = true;
            this.alpha.Location = new System.Drawing.Point(110, 56);
            this.alpha.Name = "r";
            this.alpha.Size = new System.Drawing.Size(36, 13);
            this.alpha.TabIndex = 3;
            this.alpha.Text = "α =";
            // 
            // rValue
            // 
            this.rValue.Location = new System.Drawing.Point(207, 53);
            this.rValue.Name = "rValue";
            this.rValue.Size = new System.Drawing.Size(32, 20);
            this.rValue.TabIndex = 5;
            // 
            // r
            // 
            this.r.AutoSize = true;
            this.r.Location = new System.Drawing.Point(182, 56);
            this.r.Name = "r";
            this.r.Size = new System.Drawing.Size(36, 13);
            this.r.TabIndex = 3;
            this.r.Text = "R =";
            // 
            // k
            // 
            this.k.AutoSize = true;
            this.k.Location = new System.Drawing.Point(260, 56);
            this.k.Name = "k";
            this.k.Size = new System.Drawing.Size(32, 13);
            this.k.TabIndex = 0;
            this.k.Text = "k =";
            // 
            // kValue
            // 
            this.kValue.Location = new System.Drawing.Point(282, 53);
            this.kValue.Name = "kValue";
            this.kValue.Size = new System.Drawing.Size(32, 20);
            this.kValue.TabIndex = 6;


            // 
            // showCheck
            // 
            this.showCheck.Location = new System.Drawing.Point(350, 5);
            this.showCheck.Name = "showCheck";
            this.showCheck.Size = new System.Drawing.Size(75, 70);
            this.showCheck.TabIndex = 7;
            this.showCheck.Text = "Create network";
            this.showCheck.UseVisualStyleBackColor = true;
            this.showCheck.Click += new System.EventHandler(this.showCheck_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(450, 5);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 70);
            this.test.TabIndex = 7;
            this.test.Text = "Test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            //
            // teach
            //
            this.teach.Location = new System.Drawing.Point(0, 0);
            this.teach.Name = "tech";
            this.teach.Size = new System.Drawing.Size(75, 23);
            this.teach.TabIndex = 8;
            this.teach.Text = "Teach";
            this.teach.UseVisualStyleBackColor = true;
            this.teach.Click += new System.EventHandler(this.teach_Click);
            //
            // recognize
            //
            this.recognize.Location = new System.Drawing.Point(0, 0);
            this.recognize.Name = "recognize";
            this.recognize.Size = new System.Drawing.Size(75, 23);
            this.recognize.TabIndex = 9;
            this.recognize.Text = "Recognize";
            this.recognize.UseVisualStyleBackColor = true;
            this.recognize.Click += new System.EventHandler(this.recognize_Click);
            //
            //about
            //
            this.about.Location = new System.Drawing.Point(10, 125);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(75, 23);
            this.about.TabIndex = 9;
            this.about.Text = "About";
            this.about.UseVisualStyleBackColor = true;
            this.about.Click += new System.EventHandler(this.about_Click);
            //
            //letters
            //
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].AutoSize = true;
                    letters[j][i].Location = new System.Drawing.Point(13, 6);
                    letters[j][i].Size = new System.Drawing.Size(80, 17);
                    letters[j][i].ThreeState = true;
                    letters[j][i].UseVisualStyleBackColor = true;
                }
                this.standart[i].AutoSize = true;
                this.standart[i].Location = new System.Drawing.Point(13, 6);
                this.standart[i].Size = new System.Drawing.Size(80, 17);
                this.standart[i].ThreeState = true;
                this.standart[i].UseVisualStyleBackColor = true;
            }
            // 
            // loading
            // 
            this.loading.Location = new System.Drawing.Point(125, 100);
            this.loading.Name = "loading";
            this.loading.Size = new System.Drawing.Size(333, 23);
            this.loading.TabIndex = 0;
            this.loading.Visible = false;

            //
            //loadLabel
            //
            this.loadLabel.AutoSize = true;
            this.loadLabel.Location = new System.Drawing.Point(125, 85);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(32, 13);
            this.loadLabel.TabIndex = 5;
            this.loadLabel.Visible = false;
            this.loadLabel.Text = "Progress:";
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
            //toRecSign
            //
            this.toRecSign.AutoSize = true;
            this.toRecSign.Location = new System.Drawing.Point(5, 5);
            this.toRecSign.Name = "toRecSign";
            this.toRecSign.Size = new System.Drawing.Size(32, 13);
            this.toRecSign.TabIndex = 5;
            this.toRecSign.Text = "for rcognize";
            //
            //signs
            //
            for (int i = 0; i < signs.Length; i++)
            {
                this.signs[i].AutoSize = true;
                //this.signs[i].BackColor = System.Drawing.Color.Cyan;
                this.signs[i].Location = new System.Drawing.Point(0, 0);
                this.signs[i].Name = "signs" + i.ToString();
                this.signs[i].Size = new System.Drawing.Size(35, 13);
                this.signs[i].TabIndex = 0;
                this.signs[i].Text = "Sign №" + (i + 1);
            }







            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.pictureBox1);

            this.Controls.Add(this.showCheck);
            this.Controls.Add(this.test);
            this.Controls.Add(this.teach);
            this.Controls.Add(this.recognize);
            this.Controls.Add(this.about);
            this.Controls.Add(this.width);
            this.Controls.Add(this.widthValue);
            this.Controls.Add(this.height);
            this.Controls.Add(this.heightValue);
            this.Controls.Add(this.k);
            this.Controls.Add(this.r);
            this.Controls.Add(this.kValue);
            this.Controls.Add(this.rValue);
            this.Controls.Add(this.alpha);
            this.Controls.Add(this.alphaValue);
            
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    this.Controls.Add(letters[j][i]);
                }
                this.Controls.Add(this.standart[i]);
            }
            for (int i = 0; i < signs.Length; i++)
            {
                this.Controls.Add(this.signs[i]);
            }
            this.Controls.Add(this.toRecSign);
            this.Controls.Add(this.loading);
            this.Controls.Add(this.loadLabel);
            this.Name = "Form1";
            this.Text = "Kohonen network";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);




        }
        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            this.loading.Visible = true;
            this.loadLabel.Visible = true;
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(5, 5);
                    standart[i].Location = new System.Drawing.Point(5, 5);
                }
            }
            //this.showCheck.Location = new System.Drawing.Point(0, 0);
            this.teach.Location = new System.Drawing.Point(10, 75);
            this.recognize.Location = new System.Drawing.Point(10, 100);
            int width = letterWidth;
            count = letterHeight * letterWidth;
            int y = 130;
            int xConst = 110;
            int x = xConst;
            int step = 15;
            int offset = letterWidth * step + step;
            for (int i = 0; i < count; i++)
            {
                if (i + 1 > width)
                {
                    x = xConst;
                    y += step;
                    width += letterWidth;

                }
                x += step;
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(x + j * offset, y);
                }
                this.standart[i].Location = new System.Drawing.Point(x + letters.Count * offset + step, y);
            }
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i].Location = new System.Drawing.Point(letters[i][0].Location.X, y + 15);
            }
            toRecSign.Location = new System.Drawing.Point(standart[0].Location.X, y + 15);
        }
        #endregion


        private System.Windows.Forms.PictureBox pictureBox1;

        protected System.Windows.Forms.Label height;
        protected System.Windows.Forms.Label width;
        protected System.Windows.Forms.Label alpha;
        protected System.Windows.Forms.Label k;
        protected System.Windows.Forms.Label r;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected System.Windows.Forms.TextBox kValue;
        protected System.Windows.Forms.TextBox rValue;
        protected System.Windows.Forms.TextBox alphaValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;

        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;
        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;


        protected int count;
    }
}

