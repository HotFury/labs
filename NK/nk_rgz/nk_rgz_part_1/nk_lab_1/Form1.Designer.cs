﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
public static class Constants
{
    public const int lettersCount = 5;
    public const int maxInputCount = 150;
    private static int neuronCount;
    private static int maxEpoch;
    public static int MaxEpoch
    {
        get
        {
            return maxEpoch;
        }
    }
    public static int NeuronCount
    {
        get
        {
            return neuronCount;
        }
    }

    public static List<int[]> outStandart ;

    private static readonly int[] outSt1three = {  1,  1, 1};
    private static readonly int[] outSt2three = { -1,  1, 1};
    private static readonly int[] outSt3three = {  1, -1, 1};
    private static readonly int[] outSt4three = { -1, -1, 1};
    private static readonly int[] outSt5three = { -1, -1,-1};

    private static readonly int[] outSt1five = { 1,-1,-1,-1,-1 };
    private static readonly int[] outSt2five = {-1, 1,-1,-1,-1 };
    private static readonly int[] outSt3five = {-1,-1, 1,-1,-1 };
    private static readonly int[] outSt4five = {-1,-1,-1, 1,-1 };
    private static readonly int[] outSt5five = {-1,-1,-1,-1, 1 };
    public static void InitConstants(int neurCnt, int maxEpochCount)
    {
        outStandart = new List<int[]>();
        neuronCount = neurCnt;
        maxEpoch = maxEpochCount;
        if(neuronCount == 3 )
        {
            outStandart.Add(outSt1three);
            outStandart.Add(outSt2three);
            outStandart.Add(outSt3three);
            outStandart.Add(outSt4three);
            outStandart.Add(outSt5three);
        }
        else if (neuronCount == 5)
        {
            outStandart.Add(outSt1five);
            outStandart.Add(outSt2five);
            outStandart.Add(outSt3five);
            outStandart.Add(outSt4five);
            outStandart.Add(outSt5five);
        }
    }
                                               
}
namespace nk_lab_1
{
    partial class Neuron
    {
        private int[] weight;
        private int neuronOut;
        public int NeuronOut
        {
            get
            {
                return neuronOut;
            }
            set
            {
                neuronOut = value;
            }
        }
        public int[] Weight
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
        public void InitWeight(int count)
        {
            weight = new int[count];
        }

    }
    partial class NeuronNet
    {
        private int[] input;
        private Neuron[] neurons = new Neuron[Constants.NeuronCount];
        private int epoch;
        private bool succTeach = false;
        public int Epoch
        {
            get
            {
                return epoch;
            }
        }
        public NeuronNet(int size)
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i] = new Neuron();
                neurons[i].InitWeight(size);
                
            }
            epoch = 0;
        }
        private bool ArrayEqual(int[] arr1, int[] arr2)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }
            return true;
        }
        private void InitInput(int[] inp)
        {
            int size = inp.Length + 1;
            input = new int[size];
            input[0] = 1;
            for (int i = 0; i < input.Length-1; i++)
            {
                input[i+1] = inp[i];
            }
        }
        private void InitOut(int[] outPut)
        {
            //neuronOut = outPut;
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i].NeuronOut = outPut[i];
            }
        }
        private void CorrectWeight(ref System.Diagnostics.Stopwatch teachTime)
        {
            teachTime.Start();
            for (int j = 0; j < neurons.Length; j++)
            {
                for (int i = 0; i < neurons[j].Weight.Length; i++)
                {
                    neurons[j].Weight[i] += input[i] * neurons[j].NeuronOut;
                }
                teachTime.Stop();
                WriteToLog(neurons[j].Weight, "weight", j);
            }
        }
        private int[] CheckStandart(int[] let)
        {
            int[] res = new int[neurons.Length];
            for (int i = 0; i < res.Length; i++)
            {
                int s = 0;
                for (int j = 0; j < let.Length; j++)
                {
                    s += let[j] * neurons[i].Weight[j+1];
                }
                s += neurons[i].Weight[0];
                if (s > 0)
                    res[i] = 1;
                else
                    res[i] = -1;
            }
            return res;
        }
        public void Recognize(int[] letter, List<int[]> standarts)
        {
            System.Diagnostics.Stopwatch recTime = new System.Diagnostics.Stopwatch();
            recTime.Start();
            bool recognized = false;
            for (int i = 0; i < standarts.Count; i++)
            {
                if (ArrayEqual(CheckStandart(letter),standarts[i]))
                {
                    recTime.Stop();
                    System.Windows.Forms.MessageBox.Show("Sign № " + (i + 1).ToString() + ". Recognize time: " + String.Format("{0:0.0000}",((decimal)recTime.ElapsedTicks * 1000 / System.Diagnostics.Stopwatch.Frequency)) + " mS");
                    recognized = true;
                    break;
                }
            }
            if (!recognized)
            {
                System.Windows.Forms.MessageBox.Show("Can't recognize");
            }
            
        }
        public bool Teach(List<int[]>inputs, List<int[]> outStandart, ref System.Windows.Forms.ProgressBar progBar, ref System.Diagnostics.Stopwatch teachTime)
        {
            progBar.Maximum++;
            progBar.PerformStep();
            teachTime.Start();
            epoch++;
            for (int i = 0; i < inputs.Count; i++)
            {
                if (!ArrayEqual(CheckStandart(inputs[i]), outStandart[i]))
                {
                    teachTime.Stop();
                    WriteToLog(inputs[i], "let", i);
                    InitInput(inputs[i]);
                    teachTime.Start();
                    InitOut(outStandart[i]);
                    teachTime.Stop();
                    CorrectWeight(ref teachTime);
                    teachTime.Start();
                    for (int j = 0; j < inputs.Count; j++)
                    {
                        
                        if (!ArrayEqual(CheckStandart(inputs[j]), outStandart[j]))
                        {
                            if (epoch < Constants.MaxEpoch)
                            {
                                succTeach = true;
                                teachTime.Stop();
                                WriteToLog(CheckStandart(inputs[j]), "result  ", j);
                                WriteToLog(outStandart[j], "standard", j);
                                Teach(inputs, outStandart, ref progBar, ref teachTime);
                            }
                            else
                            {
                                succTeach = false;
                            }
                        }
                    }
                }
            }
            teachTime.Stop();
            return succTeach;
        }
        public void WriteToLog(int[] arr, string arrayName, int curIter)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            string inStr = ArrayToString(arr, "|");
            string[] values = inStr.Split('|');
            string outStr = arrayName + "(" + (curIter + 1).ToString() + ")";
            foreach (string val in values)
            {
                outStr += String.Format("{0,3:0.0}", val);
            }
            write_text.WriteLine( outStr); //Записываем в файл текст из текстового поля
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }

        public string ArrayToString(int[] array, string delimiter)
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
        protected System.ComponentModel.IContainer components = null;

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
        protected void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.heightValue = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.widthValue = new System.Windows.Forms.TextBox();
            this.showCheck = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.showCheck = new System.Windows.Forms.Button();
            this.signs = new System.Windows.Forms.Label[Constants.lettersCount];
            for (int i = 0; i < Constants.lettersCount; i++ )
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

            this.toRecSign = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();

            this.threeNeuronInit = new System.Windows.Forms.RadioButton();
            this.fiveNeuronInit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.maxEpochCountInit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.loading = new System.Windows.Forms.ProgressBar();
            this.loadLabel = new System.Windows.Forms.Label();


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
            // showCheck
            // 
            this.showCheck.Location = new System.Drawing.Point(285, 1);
            this.showCheck.Name = "showCheck";
            this.showCheck.Size = new System.Drawing.Size(75, 23);
            this.showCheck.TabIndex = 6;
            this.showCheck.Text = "Create";
            this.showCheck.UseVisualStyleBackColor = true;
            this.showCheck.Click += new System.EventHandler(this.showCheck_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(385, 1);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 23);
            this.test.TabIndex = 6;
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
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 63);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;

            //
            //letters
            //
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++ )
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
            // threeNeuronInit
            // 
            this.threeNeuronInit.AutoSize = true;
            this.threeNeuronInit.Location = new System.Drawing.Point(176, 26);
            this.threeNeuronInit.Name = "threeNeuronInit";
            this.threeNeuronInit.Size = new System.Drawing.Size(31, 17);
            this.threeNeuronInit.TabIndex = 0;
            this.threeNeuronInit.TabStop = true;
            this.threeNeuronInit.Text = "3";
            this.threeNeuronInit.UseVisualStyleBackColor = true;
            // 
            // fiveNeuronInit
            // 
            this.fiveNeuronInit.AutoSize = true;
            this.fiveNeuronInit.Location = new System.Drawing.Point(213, 26);
            this.fiveNeuronInit.Name = "fiveNeuronInit";
            this.fiveNeuronInit.Size = new System.Drawing.Size(31, 17);
            this.fiveNeuronInit.TabIndex = 1;
            this.fiveNeuronInit.TabStop = true;
            this.fiveNeuronInit.Text = "5";
            this.fiveNeuronInit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Neuron count:";
            // 
            // maxEpochCountInit
            // 
            this.maxEpochCountInit.Location = new System.Drawing.Point(387, 25);
            this.maxEpochCountInit.Name = "maxEpochCountInit";
            this.maxEpochCountInit.Size = new System.Drawing.Size(41, 20);
            this.maxEpochCountInit.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maximum epoch count:";
            //
            //signs
            //
            for (int i = 0; i < signs.Length; i++)
            {
                this.signs[i].AutoSize = true;
                this.signs[i].Location = new System.Drawing.Point(0, 0);
                this.signs[i].Name = "signs" + i.ToString();
                this.signs[i].Size = new System.Drawing.Size(35, 13);
                this.signs[i].TabIndex = 0;
                this.signs[i].Text = "Sing №" + (i + 1);
            }
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
            // loading
            // 
            this.loading.Location = new System.Drawing.Point(125, 70);
            this.loading.Name = "loading";
            this.loading.Size = new System.Drawing.Size(333, 23);
            this.loading.TabIndex = 0;
            this.loading.Visible = false;

            //
            //loadLabel
            //
            this.loadLabel.AutoSize = true;
            this.loadLabel.Location = new System.Drawing.Point(125, 55);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(32, 13);
            this.loadLabel.TabIndex = 5;
            this.loadLabel.Visible = false;
            this.loadLabel.Text = "Progress:";


            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 438);
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


            this.Controls.Add(this.label2);
            this.Controls.Add(this.maxEpochCountInit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fiveNeuronInit);
            this.Controls.Add(this.threeNeuronInit);


            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++ )
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
            this.Text = "Hebb network";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            this.loading.Visible = true;
            this.loadLabel.Visible = true;
            for (int i = 0; i < Constants.maxInputCount; i++ )
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(5,5);
                    standart[i].Location = new System.Drawing.Point(5, 5);
                    letters[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standart[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
            }
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i].Location = new System.Drawing.Point(5, 5);
            }
            toRecSign.Location = new System.Drawing.Point(5, 5);
                //this.showCheck.Location = new System.Drawing.Point(0, 0);
            this.teach.Location = new System.Drawing.Point(10, 75);
            this.recognize.Location = new System.Drawing.Point(10, 100);
            int width = letterWidth;
            count  = letterHeight * letterWidth;
            int y = 100;
            int xConst = 110;
            int x = xConst;
            int step = 15;
            int offset = letterWidth * step + step;
            for (int i = 0; i < count; i++)
            {
                if (i+1 > width)
                {
                    x = xConst;
                    y += step;
                    width += letterWidth;
                    
                }
                x += step;
                for (int j = 0; j < letters.Count; j++ )
                {
                    letters[j][i].Location = new System.Drawing.Point(x + j * offset , y);
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
        
        protected System.Windows.Forms.Label height;
        protected System.Windows.Forms.Label width;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;
        private System.Windows.Forms.PictureBox pictureBox1;


        protected System.Windows.Forms.RadioButton threeNeuronInit;
        protected System.Windows.Forms.RadioButton fiveNeuronInit;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.TextBox maxEpochCountInit;
        private System.Windows.Forms.Label label2;


        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;
        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;
        protected int count;

    }
}

