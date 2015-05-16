﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
public static class Constants
{
    public const int lettersCount = 4;
    public const int maxInputCount = 30;
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

    private static readonly int[] outSt1two = {  1,  1};
    private static readonly int[] outSt2two = { -1,  1};
    private static readonly int[] outSt3two = {  1, -1};
    private static readonly int[] outSt4two = { -1, -1};

    private static readonly int[] outSt1four = { 1,-1,-1,-1 };
    private static readonly int[] outSt2four = {-1, 1,-1, 1 };
    private static readonly int[] outSt3four = {-1,-1, 1,-1 };
    private static readonly int[] outSt4four = {-1,-1,-1, 1 };
    public static void InitConstants(int neurCnt, int maxEpochCount)
    {
        outStandart = new List<int[]>();
        neuronCount = neurCnt;
        maxEpoch = maxEpochCount;
        if(neuronCount == 2 )
        {
            outStandart.Add(outSt1two);
            outStandart.Add(outSt2two);
            outStandart.Add(outSt3two);
            outStandart.Add(outSt4two);
        }
        else if (neuronCount == 4)
        {
            outStandart.Add(outSt1four);
            outStandart.Add(outSt2four);
            outStandart.Add(outSt3four);
            outStandart.Add(outSt4four);
        }
        /*outStandart.Add(outSt1);
        outStandart.Add(outSt2);
        outStandart.Add(outSt3);
        outStandart.Add(outSt4);*/
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
        private void CorrectWeight()
        {
            for (int j = 0; j < neurons.Length; j++)
            {
                for (int i = 0; i < neurons[j].Weight.Length; i++)
                {
                    neurons[j].Weight[i] += input[i] * neurons[j].NeuronOut;
                }
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
            bool recognized = false;
            for (int i = 0; i < standarts.Count; i++)
            {
                if (ArrayEqual(CheckStandart(letter),standarts[i]))
                {
                    System.Windows.Forms.MessageBox.Show("Sign № " + (i + 1).ToString());
                    recognized = true;
                    break;
                }
            }
            if (!recognized)
            {
                System.Windows.Forms.MessageBox.Show("Can't recognize");
            }
        }
        public bool Teach(List<int[]>inputs, List<int[]> outStandart)
        {
            epoch++;
            for (int i = 0; i < inputs.Count; i++)
            {
                if (!ArrayEqual(CheckStandart(inputs[i]), outStandart[i]))
                {
                    InitInput(inputs[i]);
                    InitOut(outStandart[i]);
                    
                    CorrectWeight();
                    for (int j = 0; j < inputs.Count; j++)
                    {
                        
                        if (!ArrayEqual(CheckStandart(inputs[j]), outStandart[j]))
                        {
                            if (epoch < Constants.MaxEpoch)
                            {
                                succTeach = true;
                                WriteToLog(CheckStandart(inputs[j]), "result  ", j);
                                WriteToLog(outStandart[j], "standard", j);
                                Teach(inputs, outStandart);
                            }
                            else
                            {
                                succTeach = false;
                            }
                        }
                    }
                }
            }
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
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.showCheck = new System.Windows.Forms.Button();
            for (int i = 0; i < Constants.lettersCount; i++ )
            {
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

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();

            this.twoNeuronInit = new System.Windows.Forms.RadioButton();
            this.fourNeuronInit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.maxEpochCountInit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();

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
            // twoNeuronInit
            // 
            this.twoNeuronInit.AutoSize = true;
            this.twoNeuronInit.Location = new System.Drawing.Point(176, 26);
            this.twoNeuronInit.Name = "twoNeuronInit";
            this.twoNeuronInit.Size = new System.Drawing.Size(31, 17);
            this.twoNeuronInit.TabIndex = 0;
            this.twoNeuronInit.TabStop = true;
            this.twoNeuronInit.Text = "2";
            this.twoNeuronInit.UseVisualStyleBackColor = true;
            // 
            // fourNeuronInit
            // 
            this.fourNeuronInit.AutoSize = true;
            this.fourNeuronInit.Location = new System.Drawing.Point(213, 26);
            this.fourNeuronInit.Name = "fourNeuronInit";
            this.fourNeuronInit.Size = new System.Drawing.Size(31, 17);
            this.fourNeuronInit.TabIndex = 1;
            this.fourNeuronInit.TabStop = true;
            this.fourNeuronInit.Text = "4";
            this.fourNeuronInit.UseVisualStyleBackColor = true;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 438);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.showCheck);
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
            this.Controls.Add(this.fourNeuronInit);
            this.Controls.Add(this.twoNeuronInit);


            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++ )
                {
                    this.Controls.Add(letters[j][i]);
                }
                    this.Controls.Add(this.standart[i]);
            }
            this.Name = "Form1";
            this.Text = "Hebb network";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            for (int i = 0; i < Constants.maxInputCount; i++ )
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(5,5);
                    standart[i].Location = new System.Drawing.Point(5, 5);
                }
            }
                //this.showCheck.Location = new System.Drawing.Point(0, 0);
            this.teach.Location = new System.Drawing.Point(10, 75);
            this.recognize.Location = new System.Drawing.Point(10, 100);
            int width = letterWidth;
            count  = letterHeight * letterWidth;
            int y = 50;
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
        }


        #endregion
        
        protected System.Windows.Forms.Label height;
        protected System.Windows.Forms.Label width;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;
        private System.Windows.Forms.PictureBox pictureBox1;


        protected System.Windows.Forms.RadioButton twoNeuronInit;
        protected System.Windows.Forms.RadioButton fourNeuronInit;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.TextBox maxEpochCountInit;
        private System.Windows.Forms.Label label2;

        
        protected int count;

    }
}

