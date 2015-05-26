using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
public static class Constants
{
    public const int lettersCount = 4;
    public const int maxInputCount = 150;
}

namespace nk_lab_7
{
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
        public void WriteToLogHead(int count, string col, int length)
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
            for (int i = 0; i < count; i++)
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
        /*public void WriteToLog(Z_neuron[] values, string row, int num)
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
            foreach (Z_neuron val in values)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.000}", val.Output) + "|";
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }
        */
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
    partial class Neuron : Log
    {
        private decimal input;
        private decimal output;
        public decimal Input
        {
            get
            {
                return input;
            }
            protected set
            {
                input = value;
            }
        }
        public decimal Output
        {
            get
            {
                return output;
            }
            protected set
            {
                output = value;
            }
        }
        protected List<decimal> Weight
        {
            get
            {
                return weight;
            }
        }
        public void InitOut()
        {
            output = input;
        }
        private List<decimal> weight;
        private List<decimal> prevWeight;
        protected List<decimal> PrevWeight
        {
            get
            {
                return prevWeight;
            }
        }
        private void SetWeight(decimal val)
        {
            weight.Add(val);
            prevWeight.Add(val);
        }
        private void WeightReset()
        {
            weight = new List<decimal>();
            prevWeight = new List<decimal>();
        }
        public void WeightInit(int count, decimal val, string neurName, int num)
        {
            
            //WeightReset();
            for (int i = 0; i < count; i++)
            {
                SetWeight(val);
            }
            WriteToLog(weight.ToArray(), neurName, num);
        }
        public void WeightInit(decimal val, string neurName, int num)
        {

            //WeightReset();
            //for (int i = 0; i < count; i++)
            {
                SetWeight(val);
            }
            WriteToLog(weight.ToArray(), neurName, num);
        }
        public void WriteWeightToLog(int neuronNum, string neuronName)
        {
            WriteToLog(Weight.ToArray(), neuronName, neuronNum);
        }
        public void WritePrevWeightToLog(int neuronNum, string neuronName)
        {
            WriteToLog(PrevWeight.ToArray(), neuronName+"pr", neuronNum);
        }
        public void SaveWeights()
        {
            for (int i = 0; i < Weight.Count; i++)
            {
                prevWeight[i] = weight[i];
            }
        }
        public bool WeightsEqual()
        {
            for (int i = 0; i < weight.Count; i++)
            {
                if (prevWeight[i] != weight[i])
                {
                    return false;
                }
            }
            return true;
        }
        public Neuron()
        {
            weight = new List<decimal>();
            prevWeight = new List<decimal>();
        }
    }
    partial class Z_neuron : Neuron
    {
        public Z_neuron()
            : base()
        {

        }
        public decimal InitInput(decimal val)
        {
            return Input = val;
        }
        public decimal CalculateOut(Sensor inp, int num)
        {
            Output = inp.Input * Weight[num];
            return Output;
        }
        public void WeightCorrect(int winNum)
        {
            Weight[winNum] = Output;
        }
    }
    partial class Y_neuron : Neuron
    {
        public Y_neuron()
            : base()
        {

        }
        public void InitOut(decimal val)
        {
            Output = val;
        }
        public decimal CalculateInput(List<Z_neuron> neuron)
        {
            Input = 0;
            for (int i = 0; i < neuron.Count; i++)
            {
               Input += neuron[i].Output * Weight[i];
            }
            return Input;
        }
        public void WeightCorerct(int l, List<Z_neuron> z_neur, decimal ZNorm)
        {
            for (int i = 0; i < Weight.Count; i++)
            {
                Weight[i] = (l * z_neur[i].Output) / (l - 1 + ZNorm);
            }
        }
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
        public Sensor(int inpVal)
        {
            input = inpVal;
        }
    }
    partial class ART_1 :Log
    {
        private List<Sensor> sensor;
        private List<Z_neuron> z_neuron;
        private List<Y_neuron> y_neuron;
        private int m;
        private int n;
        //private int q;
        private decimal p;
        private int L;
        private int maxEpochCount;
        private int epochCount;
        public ART_1()
        {
            m = 1;//Constants.lettersCount;
            n = 0;
            p = 0;
            L = 0;
            epochCount = 0;
            sensor = new List<Sensor>();
            z_neuron = new List<Z_neuron>();
            y_neuron = new List<Y_neuron>();
        }
        private decimal GetVectorNorm(List<decimal> vector)
        {
            decimal norm = 0;
            foreach (int curComp in vector)
            {
                norm += curComp;
            }
            return norm;
        }
        private int GetVectorNorm(List<Sensor> vector)
        {
            int norm = 0;
            foreach (Sensor curComp in vector)
            {
                norm += curComp.Input;
            }
            return norm;
        }
        private decimal GetVectorNorm(List<Z_neuron> vector)
        {
            decimal norm = 0;
            foreach (Z_neuron curComp in vector)
            {
                norm += curComp.Output;
            }
            return norm;
        }
        public void InitNet(int standartLength, decimal pVal, int Lval, int maxEpochCountVal)
        {
            maxEpochCount = maxEpochCountVal;
            n = standartLength;
            p = pVal;
            L = Lval;
            decimal weightValY = 1m / (1 + n);
            decimal weightValZ = 1;
            WriteToLogString("========Y to Z neuron weigths initalization========");
            WriteToLogHead(m, "w", 5);
            for (int i = 0; i < n; i++)
            {
                z_neuron.Add(new Z_neuron());
                z_neuron[i].WeightInit(weightValZ, "Z", i);
            }
            /*WriteToLogString("========Z to Y neuron weigths initalization========");
            WriteToLogHead(n, "w", 5);
            //for (int i = 0; i < m; i++)
            {
                y_neuron.Add(new Y_neuron());
                y_neuron[y_neuron.Count - 1].WeightInit(n, weightValY, "Y", y_neuron.Count - 1);
            }*/
        }
        private int GetWinnerNum()
        {
            int num = 0;
            decimal max = y_neuron[num].Output;
            for (int i = 1; i < y_neuron.Count; i++)
            {
                if (y_neuron[i].Output > max)
                {
                    num = i;
                    max = y_neuron[num].Output;
                }
            }
            return num;
        }
        private int GetAltNum(int sensNorm, int winNum, decimal ZNorm)
        {
            if ((decimal)ZNorm / sensNorm < p)
            {
                y_neuron[winNum].InitOut(-1);
                winNum = GetWinnerNum();
                for (int i = 0; i < z_neuron.Count; i++)
                {
                    z_neuron[i].CalculateOut(sensor[i], winNum);
                }
                ZNorm = GetVectorNorm(z_neuron);
                GetAltNum(sensNorm, winNum, ZNorm);
            }
            return winNum;
        }
        private bool AllWeightsEqual()
        {
            for (int i = 0; i < z_neuron.Count; i++)
            {
                if (!z_neuron[i].WeightsEqual())
                {
                    return false;
                }
            }
            for (int i = 0; i < y_neuron.Count; i++)
            {
                if (!y_neuron[i].WeightsEqual())
                {
                    return false;
                }
            }
            return true;
        }
        private bool Stop()
        {
            if (AllWeightsEqual())
            {
                return true;
            }
            else if (epochCount >= maxEpochCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Teach(List<List<int>> standarts, ref System.Windows.Forms.ProgressBar progBar)
        {
            epochCount++;

            for (int j = 0; j < standarts.Count; j++)
            {
                if (epochCount == 1)
                {
                    m++;
                    decimal weightValY = 1m / (1 + n);
                    decimal weightValZ = 1;
                    WriteToLogString("========Y to Z neuron weigths initalization========");
                    WriteToLogHead(m, "w", 5);
                    for (int i = 0; i < n; i++)
                    {
                        //z_neuron.Add(new Z_neuron());
                        z_neuron[i].WeightInit(weightValZ, "Z", i);
                    }
                    WriteToLogString("========Z to Y neuron weigths initalization========");
                    WriteToLogHead(n, "w", 5);
                    //for (int i = 0; i < m; i++)
                    {
                        y_neuron.Add(new Y_neuron());
                        y_neuron[y_neuron.Count - 1].WeightInit(n, weightValY, "Y", y_neuron.Count - 1);
                    }
                }
                sensor = new List<Sensor>();
                for (int i = 0; i < n; i++)
                {
                    progBar.Maximum++;
                    progBar.PerformStep();
                    sensor.Add(new Sensor(standarts[j][i]));
                    z_neuron[i].InitInput(sensor[i].Input);
                    z_neuron[i].InitOut();
                }
                int sensNorm = GetVectorNorm(sensor);
                for (int i = 0; i < m; i++)
                {
                    progBar.Maximum++;
                    progBar.PerformStep();
                    y_neuron[i].InitOut(0m);
                    y_neuron[i].CalculateInput(z_neuron);
                    y_neuron[i].InitOut();
                }
                int winNum = GetWinnerNum();
                for (int i = 0; i < z_neuron.Count; i++)
                {
                    progBar.Maximum++;
                    progBar.PerformStep();
                    z_neuron[i].CalculateOut(sensor[i], winNum);
                }
                decimal ZNorm = GetVectorNorm(z_neuron);
                if (sensNorm != 0)
                {
                    for (int i = 0; i < y_neuron.Count; i++ )
                    {
                        progBar.Maximum++;
                        progBar.PerformStep();
                        y_neuron[i].SaveWeights();
                    }
                    for (int i = 0; i < z_neuron.Count; i++)
                    {
                        progBar.Maximum++;
                        progBar.PerformStep();
                        z_neuron[i].SaveWeights();
                    }

                    winNum = GetAltNum(sensNorm, winNum, ZNorm);
                    y_neuron[winNum].WeightCorerct(L, z_neuron, ZNorm);
                    for (int i = 0; i < z_neuron.Count; i++)
                    {
                        progBar.Maximum++;
                        progBar.PerformStep();
                        z_neuron[i].WeightCorrect(winNum);
                    }
                    WriteToLogString("========Y to Z neuron weigths correction========");
                    WriteToLogHead(m, "w", 5);
                    for (int i = 0; i < z_neuron.Count; i++)
                    {
                        progBar.Maximum++;
                        progBar.PerformStep();
                        z_neuron[i].WriteWeightToLog(i, "Z");
                    }
                    WriteToLogString("========Z to Y neuron weigths correctinon========");
                    WriteToLogHead(m, "w", 5);
                    for (int i = 0; i < y_neuron.Count; i++)
                    {
                        progBar.Maximum++;
                        progBar.PerformStep();
                        y_neuron[i].WriteWeightToLog(i, "Y");
                    }
                }
            }
            if (!Stop())
            {
                Teach(standarts, ref progBar);
            }
        }
        public int Recognize(List<int> letter)
        {
            sensor = new List<Sensor>();
            for (int i = 0; i < n; i++)
            {
                /*progBar.Maximum++;
                progBar.PerformStep();*/
                sensor.Add(new Sensor(letter[i]));
                z_neuron[i].InitInput(sensor[i].Input);
                z_neuron[i].InitOut();
            }
            int sensNorm = GetVectorNorm(sensor);
            for (int i = 0; i < m; i++)
            {
                /*progBar.Maximum++;
                progBar.PerformStep();*/
                y_neuron[i].InitOut(0m);
                y_neuron[i].CalculateInput(z_neuron);
                y_neuron[i].InitOut();
            }
            return GetWinnerNum();
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.heightValue = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.widthValue = new System.Windows.Forms.TextBox();
            this.showCheck = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.signs = new System.Windows.Forms.Label[Constants.lettersCount];
            this.toRecSign = new System.Windows.Forms.Label();
            this.maxEpochValue = new TextBox();
            this.maxEpoch = new Label();
            this.LValue = new TextBox();
            this.L = new Label();
            this.pValue = new TextBox();
            this.pLabel = new Label();
            this.loading = new System.Windows.Forms.ProgressBar();
            this.loadLabel = new System.Windows.Forms.Label();
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

            this.SuspendLayout();

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
            this.showCheck.Location = new System.Drawing.Point(350, 5);
            this.showCheck.Name = "showCheck";
            this.showCheck.Size = new System.Drawing.Size(75, 70);
            this.showCheck.TabIndex = 6;
            this.showCheck.Text = "Create net";
            this.showCheck.UseVisualStyleBackColor = true;
            this.showCheck.Click += new System.EventHandler(this.showCheck_Click);

            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(450, 5);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 70);
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
            this.teach.Text = "Initialise net";
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
            this.about.Location = new System.Drawing.Point(10, 150);
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
            // pValue
            // 
            this.pValue.Location = new System.Drawing.Point(135, 53);
            this.pValue.Name = "pValue";
            this.pValue.Size = new System.Drawing.Size(32, 20);
            this.pValue.TabIndex = 4;
            // 
            // pLabel
            // 
            this.pLabel.AutoSize = true;
            this.pLabel.Location = new System.Drawing.Point(110, 56);
            this.pLabel.Name = "pLabel";
            this.pLabel.Size = new System.Drawing.Size(36, 13);
            this.pLabel.TabIndex = 3;
            this.pLabel.Text = "p =";
            // 
            // LValue
            // 
            this.LValue.Location = new System.Drawing.Point(198, 53);
            this.LValue.Name = "LValue";
            this.LValue.Size = new System.Drawing.Size(32, 20);
            this.LValue.TabIndex = 5;
            // 
            // L
            // 
            this.L.AutoSize = true;
            this.L.Location = new System.Drawing.Point(173, 56);
            this.L.Name = "r";
            this.L.Size = new System.Drawing.Size(36, 13);
            this.L.TabIndex = 3;
            this.L.Text = "L =";
            // 
            // maxEpoch
            // 
            this.maxEpoch.AutoSize = true;
            this.maxEpoch.Location = new System.Drawing.Point(234, 56);
            this.maxEpoch.Name = "maxEpoch";
            this.maxEpoch.Size = new System.Drawing.Size(32, 13);
            this.maxEpoch.TabIndex = 0;
            this.maxEpoch.Text = "Max epoch =";
            // 
            // maxEpochValue
            // 
            this.maxEpochValue.Location = new System.Drawing.Point(302, 53);
            this.maxEpochValue.Name = "maxEpochValue";
            this.maxEpochValue.Size = new System.Drawing.Size(32, 20);
            this.maxEpochValue.TabIndex = 5;


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
            this.pictureBox1.Image = global::nk_lab_7.Properties.Resources.Capture;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 57);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Adaptive resonance theory";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Controls.Add(this.showCheck);
            this.Controls.Add(this.test);
            this.Controls.Add(this.teach);
            this.Controls.Add(this.recognize);
            this.Controls.Add(this.about);
            this.Controls.Add(this.width);
            this.Controls.Add(this.widthValue);
            this.Controls.Add(this.height);
            this.Controls.Add(this.heightValue);
            this.Controls.Add(this.loading);
            this.Controls.Add(this.loadLabel);
            this.Controls.Add(this.LValue);
            this.Controls.Add(this.pValue);
            this.Controls.Add(this.maxEpochValue);
            this.Controls.Add(this.L);
            this.Controls.Add(this.pLabel);
            this.Controls.Add(this.maxEpoch);

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
            this.teach.Location = new System.Drawing.Point(10, 100);
            this.recognize.Location = new System.Drawing.Point(10, 125);
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
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;

        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;

        protected int count;

        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;

        protected System.Windows.Forms.TextBox pValue;
        protected System.Windows.Forms.TextBox LValue;
        protected System.Windows.Forms.TextBox maxEpochValue;

        protected System.Windows.Forms.Label maxEpoch;
        protected System.Windows.Forms.Label pLabel;
        protected System.Windows.Forms.Label L;
    }
}

