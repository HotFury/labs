using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
public static class Constants
{
    public const int lettersCount = 5;
    public const int maxInputCount = 150;
    
}
namespace nk_lab_4
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
        public void WriteToLog(A_neuron[] values, string row, int num, string whatOut)
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
            else if (whatOut == "input")
            {
                foreach (A_neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,2:0}", val.Input) + ",";
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
    partial class A_neuron : Log
    {
        private decimal[] weight;
        private int input;
        private decimal threshold;
        public decimal Threshold
        {
            get
            {
                return threshold;
            }
        }
        private decimal outputDec;
        private int output;
        public int Output
        {
            get
            {
                return output;
            }
        }
        public decimal[] Weight
        {
            get
            {
                return weight;
            }
            protected set
            {
                weight = value;
            }
        }
        public int Input
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
        public decimal OutputDec
        {
            get
            {
                return outputDec;
            }
            protected set
            {
                outputDec = value;
            }
        }
        public A_neuron(int inputSize)
        {
            weight = new decimal[inputSize];
            threshold = 0;
        }
        public void InitWeight(List<int[]> letters, int neuronNum)
        {
            for (int i = 0; i < weight.Length; i++)
            {
                if (i == neuronNum)
                {
                    weight[i] = 0;
                }
                else
                {
                    for (int j = 0; j < letters.Count; j++)
                        weight[i] += letters[j][i] * letters[j][neuronNum];
                }
            }
        }
        public void InitThreshold()
        {
            threshold = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                threshold += weight[i];
            }
            threshold /= Constants.lettersCount;
        }
        public void InitNeuron(int letterComponent)
        {
            input = letterComponent;
            output = 0;
        }
        public void CalculateOut(int[] inputVector)
        {
            
            outputDec = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                outputDec += inputVector[i] * weight[i];
            }
            //outputDec -= threshold;


            if (outputDec > threshold)
            {
                output = 1;
            }
            else if (outputDec == threshold)
            {
                output = input;
            }
            else if (outputDec < threshold)
            {
                output = -1;
            }
            /*if (output != input)
                input = output;*/
             
        }
        
    }
    partial class HopfieldNet : Log
    {
        private A_neuron[] a_neuron;
        private int[] input;
        private string mode;
        public string Mode
        {
            get
            {
                return mode;
            }
        }
        public int[] Input
        {
            get
            {
                return input;
            }
        }
        private List<List<int>> iterations = new List<List<int>>();
        private List<List<int>> lettersHistory = new List<List<int>>();
        public List<List<int>> Iterations 
        {
            get
            {
                return iterations;
            }

        }
        public List<List<int>> LettersHistory
        {
            get
            {
                return lettersHistory;
            }

        }
        public HopfieldNet(int inputSize, int lettersCount)
        {
            a_neuron = new A_neuron[inputSize];
            for (int i = 0; i < a_neuron.Length; i++)
                a_neuron[i] = new A_neuron(inputSize);
            input = new int[inputSize];

        }
        public void InitNet(List<int[]> letters, ref System.Windows.Forms.ProgressBar progBar, ref System.Diagnostics.Stopwatch teachTime)
        {
            WriteToLogHead(a_neuron[0].Weight, "A", 5);
            teachTime.Start();
            for (int i = 0; i < a_neuron.Length; i++)
            {
                progBar.Maximum++;
                progBar.PerformStep();
                a_neuron[i].InitWeight(letters, i);
                teachTime.Stop();
                WriteToLog(a_neuron[i].Weight, "A", i);
                teachTime.Start();
            }
            for (int i = 0; i < a_neuron.Length; i++)
            {
                teachTime.Stop();
                progBar.Maximum++;
                progBar.PerformStep();
                teachTime.Start();
                a_neuron[i].InitThreshold();

            }
            teachTime.Stop();
            WriteToLog(a_neuron, "trhld", -1,"threshold");
        }
        public void InitNeurons(int[] letter, ref System.Diagnostics.Stopwatch recTime)
        {
            List<int> neuronActiveNums = new List<int>();
            List<int> recLetter = new List<int>();
            recTime.Start();
            for (int i = 0; i < letter.Length; i++)
            {
                a_neuron[i].InitNeuron(letter[i]);
                a_neuron[i].CalculateOut(letter);
                neuronActiveNums.Add(i);
                input[i] = letter[i];
                recLetter.Add(input[i]);
            }
            recTime.Stop();
            iterations.Add(neuronActiveNums);
            lettersHistory.Add(recLetter);
        }
        public void InitMode(string modeVal)
        {
            mode = modeVal;
        }
        public void ReserIterations_LettersHistory()
        {
            iterations = new List<List<int>>();
            lettersHistory = new List<List<int>>();
        }
        private void RandWork()
        {
            List<int> activeNum = new List<int>();
            int num = new Random().Next(0, a_neuron.Length);
            a_neuron[num].InitNeuron(a_neuron[num].Output);
            a_neuron[num].CalculateOut(input);
            activeNum.Add(num);
            iterations.Add(activeNum);
            input[num] = a_neuron[num].Input;
        }
        private void AllWork()
        {
            List<int> activeNum = new List<int>();
            for (int num = 0; num < a_neuron.Length; num++)
            {
                a_neuron[num].InitNeuron(a_neuron[num].Output);
                a_neuron[num].CalculateOut(input);
                activeNum.Add(num);
                input[num] = a_neuron[num].Input;
            }
            iterations.Add(activeNum);
        }
        private void QuantWork()
        {
            List<int> activeNum = new List<int>();
            List<int> nums = new List<int>();
            for (int i = 0; i < input.Length; i++)
            {
                if (a_neuron[i].Input != a_neuron[i].Output)
                {
                    nums.Add(i);
                }
            }
            foreach (int num in nums)
            {
                a_neuron[num].InitNeuron(a_neuron[num].Output);
                a_neuron[num].CalculateOut(input);
                activeNum.Add(num);
                input[num] = a_neuron[num].Input;
            }
            iterations.Add(activeNum);
        }
        public void Recognize(ref System.Windows.Forms.ProgressBar progBar, ref System.Diagnostics.Stopwatch recTime)
        {
            progBar.Maximum++;
            progBar.PerformStep();
            WriteToLog(input, "inp", -1);
            WriteToLog(a_neuron, "out", -1, "output");
            recTime.Start();
            if (!InputOutputEqual())
            {
                switch (mode)
                {
                    case "allWork":
                        AllWork();
                        break;
                    case "randWork":
                        RandWork();
                        break;
                    case "quantWork":
                        QuantWork();
                        break;
                }
                List<int> recLet = new List<int>();
                for (int i = 0; i < input.Length; i++)
                {
                    recLet.Add(input[i]);
                }
                recTime.Stop();
                lettersHistory.Add(recLet);
                Recognize(ref progBar, ref recTime);
            }
        }
        public bool InputOutputEqual()
        {
            for (int i = 0; i < a_neuron.Length; i++)
            {
                if (a_neuron[i].Input != a_neuron[i].Output)
                {
                    return false;
                }
            }
            return true;
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
        public void MakeVisual(int num)
        {
            neuronVisual[num].BackColor = System.Drawing.Color.GreenYellow;
        }
        public void MakeVisualLet(int num, int val)
        {
            if (val == 1)
                standartRecognized[num].CheckState = System.Windows.Forms.CheckState.Indeterminate;
            else
                standartRecognized[num].CheckState = System.Windows.Forms.CheckState.Unchecked;
        }
        public void ResetVisual()
        {
            for (int i = 0; i < neuronVisual.Length; i++)
            {
                neuronVisual[i].BackColor = System.Drawing.Color.Cyan;
            }
        }
        public void DrawLettersHistory(List<List<int>> letters)
        {
            foreach(List<int> let in letters)
            {
                for (int i = 0; i < let.Count; i++)
                {
                    if (let[i] == 1)
                    {
                        standartRecognized[i].CheckState = CheckState.Indeterminate;
                    }
                    else
                    {
                        standartRecognized[i].CheckState = CheckState.Unchecked;
                    }
                }
            }
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.showCheck = new System.Windows.Forms.Button();
            this.neuronVisual = new System.Windows.Forms.Label[Constants.maxInputCount];
            this.allWork = new System.Windows.Forms.RadioButton();
            this.randWork = new System.Windows.Forms.RadioButton();
            this.quantWork = new System.Windows.Forms.RadioButton();
            this.signs = new System.Windows.Forms.Label[Constants.lettersCount];
            this.toRecSign = new System.Windows.Forms.Label();
            this.recSign = new System.Windows.Forms.Label();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                System.Windows.Forms.CheckBox[] letter = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
                signs[i] = new System.Windows.Forms.Label();
                for (int j = 0; j < Constants.maxInputCount; j++)
                {
                    letter[j] = new System.Windows.Forms.CheckBox();
                    neuronVisual[j] = new System.Windows.Forms.Label();
                }
                this.letters.Add(letter);
            }
            this.standart = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.standart[i] = new System.Windows.Forms.CheckBox();
            }

            this.standartRecognized = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.standartRecognized[i] = new System.Windows.Forms.CheckBox();
            }
            this.visLabel = new System.Windows.Forms.Label();

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


                this.standartRecognized[i].AutoSize = true;
                this.standartRecognized[i].Location = new System.Drawing.Point(13, 6);
                this.standartRecognized[i].Size = new System.Drawing.Size(80, 17);
                this.standartRecognized[i].ThreeState = true;
                this.standartRecognized[i].UseVisualStyleBackColor = true;
            }
            // 
            // neuronVisual
            // 
            for (int i = 0; i < neuronVisual.Length; i++)
            {
                
                this.neuronVisual[i].AutoSize = true;
                this.neuronVisual[i].BackColor = System.Drawing.Color.Cyan;
                this.neuronVisual[i].Location = new System.Drawing.Point(0, 0);
                this.neuronVisual[i].Name = "neuronVisual" + i.ToString();
                this.neuronVisual[i].Size = new System.Drawing.Size(35, 13);
                this.neuronVisual[i].TabIndex = 0;
                if (i < 10)
                    this.neuronVisual[i].Text = "  A" + i + "  ";
                else if (i >= 10 && i < 100)
                    this.neuronVisual[i].Text = " A" + i + " ";
                else
                    this.neuronVisual[i].Text = "A" + i;

            }
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
                this.signs[i].Text = "Sign №" + (i + 1) ;
            }
            //
            //recSign
            //
            this.recSign.AutoSize = true;
            this.recSign.Location = new System.Drawing.Point(5, 5);
            this.recSign.Name = "recSign";
            this.recSign.Size = new System.Drawing.Size(32, 13);
            this.recSign.TabIndex = 5;
            this.recSign.Text = "rcognized";
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
            //visLabel
            //
            this.visLabel.AutoSize = true;
            this.visLabel.Location = new System.Drawing.Point(5, 5);
            this.visLabel.Name = "toRecSign";
            this.visLabel.Size = new System.Drawing.Size(32, 13);
            this.visLabel.TabIndex = 5;
            this.visLabel.Text = "Neurons activity";


            // 
            // allWork
            // 
            this.allWork.AutoSize = true;
            this.allWork.Location = new System.Drawing.Point(130, 30);
            this.allWork.Name = "allWork";
            this.allWork.Size = new System.Drawing.Size(31, 17);
            this.allWork.TabIndex = 0;
            this.allWork.TabStop = true;
            this.allWork.Text = "all neurons work";
            this.allWork.UseVisualStyleBackColor = true;

            // 
            // randWork
            // 
            this.randWork.AutoSize = true;
            this.randWork.Location = new System.Drawing.Point(130, 45);
            this.randWork.Name = "randWork";
            this.randWork.Size = new System.Drawing.Size(31, 17);
            this.randWork.TabIndex = 0;
            this.randWork.TabStop = true;
            this.randWork.Text = "random neuron work";
            this.randWork.UseVisualStyleBackColor = true;

            // 
            // quantWork
            // 
            this.quantWork.AutoSize = true;
            this.quantWork.Location = new System.Drawing.Point(130, 60);
            this.quantWork.Name = "quantWork";
            this.quantWork.Size = new System.Drawing.Size(31, 17);
            this.quantWork.TabIndex = 0;
            this.quantWork.TabStop = true;
            this.quantWork.Text = "quantity of neurons work";
            this.quantWork.UseVisualStyleBackColor = true;


            // 
            // loading
            // 
            this.loading.Location = new System.Drawing.Point(125, 115);
            this.loading.Name = "loading";
            this.loading.Size = new System.Drawing.Size(333, 23);
            this.loading.TabIndex = 0;
            this.loading.Visible = false;

            //
            //loadLabel
            //
            this.loadLabel.AutoSize = true;
            this.loadLabel.Location = new System.Drawing.Point(125, 100);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.Size = new System.Drawing.Size(32, 13);
            this.loadLabel.TabIndex = 5;
            this.loadLabel.Visible = false;
            this.loadLabel.Text = "Progress:";


            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 88);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 








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
            this.Controls.Add(this.allWork);
            this.Controls.Add(this.randWork);
            this.Controls.Add(this.quantWork);
            this.Controls.Add(this.visLabel);
            
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    this.Controls.Add(letters[j][i]);
                }
                this.Controls.Add(this.standart[i]);
                this.Controls.Add(this.standartRecognized[i]);
                this.Controls.Add(this.neuronVisual[i]);
            }
            for (int i = 0; i < signs.Length; i++)
            {
                this.Controls.Add(this.signs[i]);
            }
            this.Controls.Add(this.recSign);
            this.Controls.Add(this.toRecSign);
            this.Controls.Add(this.loading);
            this.Controls.Add(this.loadLabel);
            this.Name = "Form1";
            this.Text = "Hopfield network";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);




        }
        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            this.loading.Visible = true;
            this.loadLabel.Visible = true;
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.neuronVisual[i].Location = new System.Drawing.Point(5, 5);
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(5, 5);
                    standart[i].Location = new System.Drawing.Point(5, 5);
                    standartRecognized[i].Location = new System.Drawing.Point(5, 5);
                    letters[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standart[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standartRecognized[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
            }
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i].Location = new System.Drawing.Point(5, 5);
            }
            toRecSign.Location = new System.Drawing.Point(5, 5);
            recSign.Location = new System.Drawing.Point(5, 5);
            visLabel.Location = new System.Drawing.Point(5, 5);
            this.teach.Location = new System.Drawing.Point(10, 100);
            this.recognize.Location = new System.Drawing.Point(10, 125);
            int width = letterWidth;
            count = letterHeight * letterWidth;
            int y = 150;
            int xConst = 110;
            int x = xConst;
            int step = 15;
            int labelX = 100;
            int labelY = y + letterHeight * 15 + 50;
            int xx = labelX;
            int yy = labelY + 20;
            int offset = letterWidth * step + step;
            int labelOffset = 0;
            for (int i = 0; i < count; i++)
            {
                labelOffset++;
                if (i + 1 > width)
                {
                    labelOffset = 1;
                    x = xConst;
                    y += step;
                    width += letterWidth;
                    xx = labelX;
                    yy += step;

                }
                x += step;
                this.neuronVisual[i].Location = new System.Drawing.Point(xx + labelOffset * 35, yy);
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(x + j * offset, y);
                }
                this.standart[i].Location = new System.Drawing.Point(x + letters.Count * offset + step, y);
                this.standartRecognized[i].Location = new System.Drawing.Point(x + (letters.Count + 1) * offset + step, y);
            }
            visLabel.Location = new System.Drawing.Point(neuronVisual[0].Location.X, neuronVisual[0].Location.Y - 20);
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i].Location = new System.Drawing.Point(letters[i][0].Location.X, y + 15);
            }
            toRecSign.Location = new System.Drawing.Point(standart[0].Location.X, y + 15);
            recSign.Location = new System.Drawing.Point(standartRecognized[0].Location.X, y + 15);

        }
        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;

        protected System.Windows.Forms.Label height;
        protected System.Windows.Forms.Label width;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.CheckBox[] standartRecognized;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;
        private System.Windows.Forms.Label[] neuronVisual;
        protected System.Windows.Forms.RadioButton allWork;
        protected System.Windows.Forms.RadioButton randWork;
        protected System.Windows.Forms.RadioButton quantWork;
        protected int count;

        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;
        protected System.Windows.Forms.Label recSign;
        protected System.Windows.Forms.Label visLabel;

        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;
    }
}

