using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
public static class Constants
{
    public const int lettersCount = 2;
    public const int maxInputCount = 150;
    
    private static bool alphaSystem;
    private static bool gammaSystem;
    public static bool AlphaSystem
    {
        get
        {
            return alphaSystem;
        }
    }
    public static bool GammaSystem
    {
        get
        {
            return gammaSystem;
        }
    }
    public static void InitConstants(string sys)
    {
        if (sys == "alpha")
        {
            alphaSystem = true;
        }
        else if (sys == "gamma")
        {
            gammaSystem = true;
        }
    }
}
namespace nk_lab_2
{
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
    partial class Associator
    {
        private decimal input;
        public decimal Input
        {
            get
            {
                return input;
            }
        }
        private decimal[] weight;
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
        private int output;
        public int Output
        {
            get
            {
                return output;
            }
        }
        public Associator(int connectionCount)
        {
            weight = new decimal[connectionCount];
            input = 0;
            output = 0;
        }
        public void InitInput(Sensor[] sensor)
        {
            input = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                input += sensor[i].Input * weight[i];
            }
        }
        public void CalculateOut(decimal limit)
        {
            if (input >= limit)
                output = 1;
            else
                output = 0;
        }
        /*public decimal[] SaveWeight()
        {
            decimal[] res = new decimal[weight.Length];
            for (int i = 0; i < weight.Length; i++)
            {
                res[i] = weight[i];
            }
            return res;
        }
        public void LoadWeigth(decimal[] newWeight)
        {
            for (int i = 0; i < weight.Length; i++)
            {
                weight[i] = newWeight[i];
            }
        }*/
    }
    partial class Responder
    {
        private decimal input;
        public decimal Input
        {
            get
            {
                return input;
            }
        }
        private int output;
        public int Output
        {
            get
            {
                return output;
            }
        }
        private decimal[] weight;
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
        public Responder(int connectionCount)
        {
            weight = new decimal[connectionCount];
            input = 0;
            output = 0;
        }
        public void InitInput(Associator[] associator)
        {
            input = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                
                input += associator[i].Output * weight[i];
            }
        }
        public void CalculateOut(decimal limit)
        {
            if (input >= limit)
                output = 1;
            else
                output = -1;
        }
        public decimal[] SaveWeight()
        {
            decimal[] res = new decimal[weight.Length];
            for (int i = 0; i  < weight.Length; i++)
            {
                res[i] = weight[i];
            }
            return res;
        }
        public void LoadWeigth(decimal[] newWeight)
        {
            for (int i = 0; i < weight.Length; i++)
            {
                weight[i] = newWeight[i];
            }
        }
    }
    partial class Perceptron
    {
        private Sensor[] sensor;
        public Sensor[] Sensor
        {
            get
            {
                return sensor;
            }
        }
        private Associator[] associator;
        public Associator[] Associator
        {
            get
            {
                return associator;
            }
        }
        private Responder responder;
        public Responder Responder
        {
            get
            {
                return responder;
            }
        }
        private decimal[] associatorLimit;
        private decimal responderLimit;
        private decimal step;
        private List<decimal> sum = new List<decimal>();
        public decimal Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }
        public Perceptron(int inputCount, int associatorCount)
        {
            sensor = new Sensor[inputCount];
            associator = new Associator[associatorCount];
            //InitSensor(inputCount, 0);
            InitAssociator(inputCount);
            InitResponder(associatorCount);
           

        }
        public void InitSensor(int[] value)
        {
            for (int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = new Sensor(value[i]);
            }
        }
        private void InitAssociator(int count)
        {
            for (int i = 0; i < associator.Length; i++)
            {
                associator[i] = new Associator(count);
                associatorLimit = new decimal[associator.Length];
            }
        }
        private void InitResponder(int count)
        {
            responder = new Responder(count);
        }
        public void InitWeights(List<decimal[]> associatorsWeight, decimal[] respondersWeigth)
        {
            
            for (int i = 0; i < associator.Length; i++)
            {
                for (int j = 0; j < associator[i].Weight.Length; j++)
                {
                    associator[i].Weight[j] = associatorsWeight[i][j];
                }
            }
            for (int i = 0; i < responder.Weight.Length; i++)
            {
                responder.Weight[i] = respondersWeigth[i];
            }
        }
        public int Recognize(int[] input)
        {
            InitSensor(input);
            for (int j = 0; j < Associator.Length; j++)
            {
                Associator[j].InitInput(Sensor);
                Associator[j].CalculateOut(associatorLimit[j]);
            }
            Responder.InitInput(Associator);
            Responder.CalculateOut(responderLimit);
            return Responder.Output;
        }
        public int Recognize(int[] input, out int activeCount)
        {
            activeCount = 0;
            InitSensor(input);
            for (int j = 0; j < Associator.Length; j++)
            {
                Associator[j].InitInput(Sensor);
                Associator[j].CalculateOut(associatorLimit[j]);
                if (Associator[j].Output == 1 /*&& Responder.Weight[j] != 0*/)
                {
                    activeCount++;
                }
            }
            Responder.InitInput(Associator);
            Responder.CalculateOut(responderLimit);
            return Responder.Output;
        }
        public void TeachAlpha(int[] letter1, int[] letter2, ref System.Windows.Forms.ProgressBar progBar)
        {
            InitLimits(letter1, letter2);
            int letter1Result = 1;
            int letter2Result = -1;
            int iter = 0;
            WriteToLogHead(Responder.Weight, "w2_", 5);
            for (; ; )
            {
                progBar.Maximum++;
                progBar.PerformStep();
                int recCount = 0;
                if (letter1Result != Recognize(letter1))
                {
                    CorrectWeight(step);
                    iter++;
                    WriteToLog(Responder.Weight, "+|", iter);
                }
                else
                {
                    recCount++;
                }
                
                if (letter2Result != Recognize(letter2))
                {
                    CorrectWeight(-step);
                    iter++;
                    WriteToLog(Responder.Weight, "-|", iter);
                }
                else
                {
                    recCount++;
                }
                
                if (iter >= 300)
                {
                    MessageBox.Show("Teach impossible. Reinitialisate perceptron (click 'Initialise perceptron')");
                    break;
                }
                else if (recCount == 2)
                {
                    MessageBox.Show("Teach successful");
                    break;
                }
            }
        }
        public void TeachGamma(int[] letter1, int[] letter2, ref System.Windows.Forms.ProgressBar progBar)
        {
            InitLimits(letter1, letter2);
            int letter1Result = 1;
            int letter2Result = -1;
            int iter = 0;
            WriteToLogHead(Responder.Weight, "w2_", 5);
            for (; ; )
            {
                progBar.Maximum++;
                progBar.PerformStep();
                decimal res1 = 0;
                decimal res2 = 0;
                int recCount = 0;
                int activeCount;
                if (Recognize(letter1, out activeCount) != letter1Result)
                {
                    CorrectWeight(step, activeCount);
                    iter++;
                    WriteToLog(Responder.Weight, "", iter);
                }
                else
                {
                    recCount++;
                }
                res1 = Responder.Input;
                if (Recognize(letter2, out activeCount) != letter2Result)
                {
                    
                    CorrectWeight(-step, activeCount);
                    iter++;
                    WriteToLog(Responder.Weight, "", iter);
                }
                else
                {
                    recCount++;
                }
                res2 = Responder.Input;
                sum.Add(res1 + res2);
                if (iter >= 300)
                {
                    MessageBox.Show("Teach impossible. Reinitialisate perceptron (click 'Initialise perceptron')");
                    WriteToLogString("====SUM====");
                    WriteToLog(sum.ToArray(), "sum", 1);
                    break;
                }
                else if (recCount == 2)
                {
                    MessageBox.Show("Teach successful");
                    WriteToLogString("====SUM====");
                    WriteToLog(sum.ToArray(), "sum", 1);
                    break;
                }
            }
        }
        private void CorrectWeight(decimal step)
        {
            for (int i = 0; i < Responder.Weight.Length; i++)
            {
                if (Associator[i].Output == 1)
                {
                    if(Responder.Weight[i] + step > 1)
                    {
                        Responder.Weight[i] = 1;
                    }
                    else if (Responder.Weight[i] + step < 0)
                    {
                        Responder.Weight[i] = 0;
                    }
                    else
                    {
                        Responder.Weight[i] += step;
                    }
                }
            }
        }
        private void CorrectWeight(decimal step, int activeCount)
        {
            int decrement = 0;
            for (int i = 0; i < Responder.Weight.Length; i++)
            {
                if (Responder.Weight[i] == 0 || Responder.Weight[i] == 1)
                {
                    decrement++;
                }
            }
            decimal weightActiveStep = step - ((activeCount - decrement) * step) / (Associator.Length - decrement);
            decimal weightPasiveStep = (-(activeCount - decrement) * step) / (Associator.Length - decrement);
            for (int i = 0; i < Responder.Weight.Length; i++)
            {
                if (Associator[i].Output == 1)
                {
                    if (Responder.Weight[i] + weightActiveStep > 1 )
                    {
                        Responder.Weight[i] = 1;
                    }
                    else if (Responder.Weight[i] + weightActiveStep < 0)
                    {
                        Responder.Weight[i] = 0;
                    }
                    else
                    {
                        Responder.Weight[i] += weightActiveStep;
                    }
                }
                else
                {
                    if (Responder.Weight[i] + weightPasiveStep < 0)
                    {
                        Responder.Weight[i] = 0;
                    }
                    else if (Responder.Weight[i] + weightPasiveStep > 1)
                    {
                        Responder.Weight[i] = 1;
                    }
                    else
                    {
                        Responder.Weight[i] += weightPasiveStep;
                    }
                }
            }
        }
        
        private void InitLimits(int[] letter1, int[] letter2)
        {
            List<decimal> associatorInputs1 = new List<decimal>();
            List<decimal> associatorInputs2 = new List<decimal>();
            Recognize(letter1);
            foreach (Associator assoc in Associator)
            {
                associatorInputs1.Add(assoc.Input);
            }
            Recognize(letter2);
            foreach (Associator assoc in Associator)
            {
                associatorInputs2.Add(assoc.Input);
            }
            //associatorInputs.Sort();
            for (int i = 0; i < associator.Length; i++)
            {
                associatorLimit[i] = (associatorInputs1[i] + associatorInputs2[i]) / 2;
            }
            Recognize(letter1);
            decimal res1 = responder.Input;
            //MessageBox.Show(res1.ToString());
            Recognize(letter2);
            decimal res2 = responder.Input;
            //MessageBox.Show(res2.ToString());
            //sum = res1 + res2;
            responderLimit = (res1 + res2) / 2;
            //MessageBox.Show(responderLimit.ToString());

        }
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
            string bound =  "";
            for (int i = 0; i < length; i++)
                bound += "=";
            bound += "|";
            string bounder = bound;
            string head = String.Format("{0,"+ length.ToString()+":0.000}", "") + "|";
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
        public void WriteToLog(Associator[] values, string row, int num)
        {
            StreamWriter write_text;  //Класс для записи в файл
            FileInfo file = new FileInfo("log.txt");
            write_text = file.AppendText(); //Дописываем инфу в файл, если файла не существует он создастся
            //string inString = ArrayToString(arr, "|");
            //string[] values = inString.Split('|');
            string outString = String.Format("{0,5:0.000}", row + num.ToString()) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            foreach (Associator val in values)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.0}", val.Input) + "|";
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();



            this.heightValue = new System.Windows.Forms.TextBox();
            this.height = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.widthValue = new System.Windows.Forms.TextBox();
            this.stepValue = new System.Windows.Forms.TextBox();
            this.stepLabel = new System.Windows.Forms.Label();
            this.associatorCountLabel = new System.Windows.Forms.Label();
            this.associatorCountValue = new System.Windows.Forms.TextBox();
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

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();

            this.alphaSystem = new System.Windows.Forms.RadioButton();
            this.gammaSystem = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
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
            // associatorCountValue
            // 
            this.associatorCountValue.Location = new System.Drawing.Point(207, 53);
            this.associatorCountValue.Name = "associatorCountValue";
            this.associatorCountValue.Size = new System.Drawing.Size(32, 20);
            this.associatorCountValue.TabIndex = 4;
            // 
            // associatorCountLabel
            // 
            this.associatorCountLabel.AutoSize = true;
            this.associatorCountLabel.Location = new System.Drawing.Point(106, 56);
            this.associatorCountLabel.Name = "associatorCountLabel";
            this.associatorCountLabel.Size = new System.Drawing.Size(36, 13);
            this.associatorCountLabel.TabIndex = 3;
            this.associatorCountLabel.Text = "Hide neuron count";
            // 
            // stepLabel
            // 
            this.stepLabel.AutoSize = true;
            this.stepLabel.Location = new System.Drawing.Point(265, 56);
            this.stepLabel.Name = "stepLabel";
            this.stepLabel.Size = new System.Drawing.Size(32, 13);
            this.stepLabel.TabIndex = 5;
            this.stepLabel.Text = "Step";
            // 
            // stepValue
            // 
            this.stepValue.Location = new System.Drawing.Point(296, 53);
            this.stepValue.Name = "stepValue";
            this.stepValue.Size = new System.Drawing.Size(32, 20);
            this.stepValue.TabIndex = 5;


            // 
            // showCheck
            // 
            this.showCheck.Location = new System.Drawing.Point(350, 5);
            this.showCheck.Name = "showCheck";
            this.showCheck.Size = new System.Drawing.Size(75, 70);
            this.showCheck.TabIndex = 6;
            this.showCheck.Text = "Initialise perceptron";
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
            // alphaSystem
            // 
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
                this.signs[i].Text = "Sing №" + (i + 1);
            }

            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::nk_lab_2.Properties.Resources.Capture;
            this.pictureBox1.Location = new System.Drawing.Point(2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 62);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
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
            this.Controls.Add(this.stepLabel);
            this.Controls.Add(this.stepValue);
            this.Controls.Add(this.associatorCountLabel);
            this.Controls.Add(this.associatorCountValue);


            //this.Controls.Add(this.label2);
            //this.Controls.Add(this.maxEpochCountInit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gammaSystem);
            this.Controls.Add(this.alphaSystem);


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
            this.Text = "Rozenblatt perceptron";
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
                    letters[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standart[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
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
        protected System.Windows.Forms.Label stepLabel;
        protected System.Windows.Forms.Label associatorCountLabel;
        protected System.Windows.Forms.TextBox heightValue;
        protected System.Windows.Forms.TextBox widthValue;
        protected System.Windows.Forms.TextBox stepValue;
        protected System.Windows.Forms.TextBox associatorCountValue;
        protected List<System.Windows.Forms.CheckBox[]> letters = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;

        protected System.Windows.Forms.RadioButton alphaSystem;
        protected System.Windows.Forms.RadioButton gammaSystem;
        private System.Windows.Forms.Label label1;
        //protected System.Windows.Forms.TextBox maxEpochCountInit;
        //private System.Windows.Forms.Label label2;

        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;


        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;

        protected int count;
    }
}

