using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
public static class Constants
{
    public const int lettersCount = 6;
    public const int maxInputCount = 60;
    
}
namespace nk_lab_3
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
        public void WriteToLog(Z_neuron[] values, string row, int num)
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
                outString += String.Format("{0,5:0.0}", val.Output) + "|";
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
    partial class Sensor
    {
        private int input;
        public int Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }
        public Sensor(int inputValue)
        {
            input = inputValue;
        }
    }
    partial class Neuron
    {
        private decimal[] weight;
        private decimal input;
        private decimal output;
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
        public Neuron(int count)
        {
            weight = new decimal[count];
        }

    }
    partial class Z_neuron : Neuron
    {

        private decimal offset;
        private decimal k_const;
        private decimal U_const;
        public Z_neuron(int count, decimal k) : base(count)
        {
            offset = (decimal) count / 2;
            k_const = k;
            U_const = 1m / k;
            //MessageBox.Show("k = " + k_const + " U = " + U_const);
        }
        public void InitWeight(Sensor[] sens)
        {
            for (int i = 0; i < sens.Length; i++)
            {
                Weight[i] = (decimal)sens[i].Input / 2;
            }
        }
        public decimal InitInput(Sensor[] sensor)
        {
            Input = offset;
            for (int i = 0; i < sensor.Length; i++)
            {
                Input += Weight[i] * sensor[i].Input;
            }
            return Input;
        }
        public decimal CalculateOut()
        {
            Output = 0;
            if (Input <= 0)
            {
                Output = 0;
            }
            else if (Input > 0 && Input <= U_const)
            {
                Output = k_const * Input;
            }
            else if (Input > U_const)
            {
                Output = U_const;
            }
            return Output;
        }
    }
    partial class A_neuron : Neuron
    {
        private decimal epsilon;
        public decimal Epsilon
        {
            get
            {
                return epsilon;
            }
        }
        private decimal[] input;
        public A_neuron(int count) : base(count)
        {
            epsilon = 1m / count;
            input = new decimal[count];
        }
        public void InitInput(Z_neuron[] Z)
        {
            for (int i = 0; i < Z.Length; i++)
            {
                input[i] = Z[i].Output;
            }
        }
        public void InitInput(A_neuron[] A)
        {
            for (int i = 0; i < A.Length; i++)
            {
                input[i] = A[i].Output;
            }
        }
        public void InitWeight(int neuronNum)
        {
            for (int i = 0; i < Weight.Length; i++)
            {
                if (i == neuronNum)
                {
                    Weight[i] = 1;
                }
                else
                {
                    Weight[i] = -epsilon;
                }
            }
        }
        public decimal CalculateOutput() 
        {
            Output = 0;
            for (int i = 0; i < Weight.Length; i++)
            {
                Output += input[i] * Weight[i];
            }
            if (Output < 0)
            {
                Output = 0;
            }
            return Output;
        }

    }
    partial class MaxNet : Log
    {
        private A_neuron[] a_neuron;
        private int iter;
        private decimal[] output;
        public decimal[] Output
        {
            get
            {
                return output;
            }
        }
        public MaxNet(Z_neuron[] Z)
        {
            iter = 0;
            output = new decimal[Z.Length];
            a_neuron = new A_neuron[Z.Length];
            for (int i = 0; i < a_neuron.Length; i++)
            {
                a_neuron[i] = new A_neuron(Z.Length);
                a_neuron[i].InitInput(Z);
                a_neuron[i].InitWeight(i);
            }
        }
        public void CalculateOut()
        {
            iter++;
            for (int i = 0; i < a_neuron.Length; i++)
            {
                output[i] = a_neuron[i].CalculateOutput();
            }
            for (int i = 0; i < a_neuron.Length; i++)
            {
                a_neuron[i].InitInput(a_neuron);
            }
            WriteToLog(output, "t", iter);
            int uniq = 0;
            for (int i = 0; i < output.Length; i++)
            {
                if (output[i] != 0)
                {
                    uniq += 1;
                }
            }
            if (uniq != 1)
            {
                CalculateOut();
            }
        }
        public decimal GetEpsilon()
        {
            return a_neuron[0].Epsilon;
        }
    }
    partial class Y_neuron
    {
        private int output;
        private decimal input;
        public Y_neuron(decimal inp)
        {
            InitInput(inp);
        }
        public void InitInput(decimal inp)
        {
            input = inp;
        }
        public int CalculateOut()
        {
            if (input > 0)
            {
                output = 1;
            }
            else
            {
                output = 0;
            }
            return output;
        }
    }
    partial class HebbNet : Log
    {
        private Sensor[] sensor;
        private Z_neuron[] z_neuron;
        private MaxNet maxNet;
        private Y_neuron[] y_neuron;
        private int[] output;
        public HebbNet(int lettersCount, int lettersSize, decimal k)
        {
            sensor = new Sensor[lettersSize];
            z_neuron = new Z_neuron[lettersCount];
            for (int i = 0; i < z_neuron.Length; i++ )
            {
                z_neuron[i] = new Z_neuron(lettersSize, k);
            }
            y_neuron = new Y_neuron[lettersCount];
            output = new int[lettersCount];
        }
        public void InitInput(int[] letter)
        {
            for (int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = new Sensor(letter[i]);
            }
            for (int i = 0; i < z_neuron.Length; i++)
            {
                z_neuron[i].InitInput(sensor);
            }
        }
        public void InitNet(List<int[]> letters)
        {
            WriteToLogHead(z_neuron[0].Weight, "S", 5);
            for (int i = 0; i < letters.Count; i++)
            {
                InitInput(letters[i]);
                z_neuron[i].InitWeight(sensor);
                WriteToLog(z_neuron[i].Weight, "Z", i);
            }
        }

        public int Recognize(int[] letter)
        {
            int res = -1;
            List<decimal> temp = new List<decimal>();
            InitInput(letter);
            for (int i = 0; i < z_neuron.Length; i++)
            {
                z_neuron[i].CalculateOut();
            }
            WriteToLogString("=========MaxNet input values=========");
            WriteToLog(z_neuron, "Zout", -1);
            maxNet = new MaxNet(z_neuron);
            
            for (int i = 0; i < z_neuron.Length; i++ )
            {
                temp.Add(z_neuron[i].Output);
            }
            temp.Sort();
            if (temp[temp.Count - 1] != temp[temp.Count - 2])
            {
                WriteToLogString("==============MaxNet Working==============");
                WriteToLogHead(maxNet.Output, "A", 5);
                maxNet.CalculateOut();
                for (int i = 0; i < y_neuron.Length; i++)
                {
                    y_neuron[i] = new Y_neuron(maxNet.Output[i]);
                    output[i] = y_neuron[i].CalculateOut();
                }
                for (int i = 0; i < output.Length; i++)
                {
                    if (output[i] == 1)
                    {
                        res = i;
                    }
                }
            }
            else
            {
                WriteToLogString("==============MaxNet not worked, because there are 2 or more maximum equal input==============");
            }
            return res;
        }
        public decimal GetEpsilon()
        {
            return maxNet.GetEpsilon();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.heightValue = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.widthValue = new System.Windows.Forms.TextBox();
            /*this.stepValue = new System.Windows.Forms.TextBox();*/
            this.epsilonLabel = new System.Windows.Forms.Label();
            this.epsilonValue = new System.Windows.Forms.Label();
            this.coeficient = new System.Windows.Forms.Label();
            this.coeficientValue = new System.Windows.Forms.TextBox();
            this.showCheck = new System.Windows.Forms.Button();
            this.teach = new System.Windows.Forms.Button();
            this.recognize = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.showCheck = new System.Windows.Forms.Button();
            for (int i = 0; i < Constants.lettersCount; i++)
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

            /*this.alphaSystem = new System.Windows.Forms.RadioButton();
            this.gammaSystem = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();*/

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
            // associatorCountValue
            //

            this.coeficientValue.Location = new System.Drawing.Point(207, 53);
            this.coeficientValue.Name = "associatorCountValue";
            this.coeficientValue.Size = new System.Drawing.Size(32, 20);
            this.coeficientValue.TabIndex = 4;
            // 
            // associatorCountLabel
            // 
            this.coeficient.AutoSize = true;
            this.coeficient.Location = new System.Drawing.Point(106, 56);
            this.coeficient.Name = "associatorCountLabel";
            this.coeficient.Size = new System.Drawing.Size(36, 13);
            this.coeficient.TabIndex = 3;
            this.coeficient.Text = "Coeficient value(k):";
            // 
            // epsilonLabel
            // 
            
            this.epsilonLabel.AutoSize = true;
            this.epsilonLabel.Location = new System.Drawing.Point(265, 56);
            this.epsilonLabel.Name = "stepLabel";
            this.epsilonLabel.Size = new System.Drawing.Size(32, 13);
            this.epsilonLabel.TabIndex = 5;
            this.epsilonLabel.Text = "ε = ";
            // 
            // epsilonValue
            // 
            this.epsilonValue.AutoSize = true;
            this.epsilonValue.Location = new System.Drawing.Point(290, 56);
            this.epsilonValue.Name = "stepLabel";
            this.epsilonValue.Size = new System.Drawing.Size(32, 13);
            this.epsilonValue.TabIndex = 5;
            this.epsilonValue.Text = "N/A";

            
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
            // alphaSystem
            // 
            /*
            this.alphaSystem.AutoSize = true;
            this.alphaSystem.Location = new System.Drawing.Point(216, 26);
            this.alphaSystem.Name = "alpha";
            this.alphaSystem.Size = new System.Drawing.Size(31, 17);
            this.alphaSystem.TabIndex = 0;
            this.alphaSystem.TabStop = true;
            this.alphaSystem.Text = "α";
            this.alphaSystem.UseVisualStyleBackColor = true;
            // 
            // gammaSystem
            // 
            this.gammaSystem.AutoSize = true;
            this.gammaSystem.Location = new System.Drawing.Point(253, 26);
            this.gammaSystem.Name = "gamma";
            this.gammaSystem.Size = new System.Drawing.Size(31, 17);
            this.gammaSystem.TabIndex = 1;
            this.gammaSystem.TabStop = true;
            this.gammaSystem.Text = "γ";
            this.gammaSystem.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reinforcement system:";
            */
            


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
            this.Controls.Add(this.teach);
            this.Controls.Add(this.recognize);
            this.Controls.Add(this.about);
            this.Controls.Add(this.width);
            this.Controls.Add(this.widthValue);
            this.Controls.Add(this.height);
            this.Controls.Add(this.heightValue);
            this.Controls.Add(this.epsilonLabel);
            this.Controls.Add(this.epsilonValue);
            /*this.Controls.Add(this.stepValue);*/
            this.Controls.Add(this.coeficient);
            this.Controls.Add(this.coeficientValue);


            //this.Controls.Add(this.label2);
            //this.Controls.Add(this.maxEpochCountInit);
            //this.Controls.Add(this.label1);
            //this.Controls.Add(this.gammaSystem);
            //this.Controls.Add(this.alphaSystem);


            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    this.Controls.Add(letters[j][i]);
                }
                this.Controls.Add(this.standart[i]);
            }


            this.Name = "Form1";
            this.Text = "Hemming net";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);




        }
        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(5, 5);
                    standart[i].Location = new System.Drawing.Point(5, 5);
                }
            }
            //this.showCheck.Location = new System.Drawing.Point(0, 0);
            this.teach.Location = new System.Drawing.Point(10, 100);
            this.recognize.Location = new System.Drawing.Point(10, 125);
            int width = letterWidth;
            count = letterHeight * letterWidth;
            int y = 80;
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
        }
        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;

        protected System.Windows.Forms.Label height;
        protected System.Windows.Forms.Label width;
        protected System.Windows.Forms.Label epsilonLabel;
        protected System.Windows.Forms.Label epsilonValue;
        protected System.Windows.Forms.Label coeficient;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        //protected System.Windows.Forms.TextBox stepValue;
        protected System.Windows.Forms.TextBox coeficientValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;

        //protected System.Windows.Forms.RadioButton alphaSystem;
        //protected System.Windows.Forms.RadioButton gammaSystem;
        //private System.Windows.Forms.Label label1;
        //protected System.Windows.Forms.TextBox maxEpochCountInit;
        //private System.Windows.Forms.Label label2;


        protected int count;
    }
}

