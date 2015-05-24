using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Windows.Forms;
public static class Constants
{
    public const int lettersCount = 2;
    public const int maxInputCount = 150;

}
namespace nk_lab_6
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
        public void WriteToLogHead(int lenght, string col, int length)
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
            for (int i = 0; i < lenght; i++)
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
            string outString = String.Format("{0,5:0.###}", row) + "|";
            if (num != -1)
                outString = String.Format("{0,5:0.###}", row + num.ToString()) + "|";
            string bound = "-----|";
            string bounder = "=====|";
            foreach (int val in arr)
            {
                bounder += bound;
                outString += String.Format("{0,5:0.###}", val) + "|";
            }

            write_text.WriteLine(outString); //Записываем в файл текст из текстового поля
            write_text.WriteLine(bounder);
            //write_text.WriteLine("==========");
            write_text.Close(); // Закрываем файл
        }
        public void WriteToLog(List<Neuron> values, string row, int num, string whatOut)
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
            if (whatOut == "input")
            {
                foreach (Neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,2:0}", val.Input) + ",";
                }
            }
            else if (whatOut == "output")
            {
                foreach (Neuron val in values)
                {
                    bounder += bound;
                    outString += String.Format("{0,5:0.#}", val.Output) + "|";
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
    partial class Neuron
    {
        private int inputLenght;
        private int input;
        private int output;
        public int Input
        {
            get
            {
                return input;
            }
        }
        public int Output
        {
            get
            {
                return output;
            }
        }
        private List<int> weight;
        public List<int> Weight
        {
            get
            {
                return weight;
            }
        }
        private decimal threshold;
        public Neuron(int inputVal, int inputLenghtVal)
        {
            input = inputVal;
            inputLenght = inputLenghtVal;
            output = 0;
            threshold = 0m;
        }
        public int CalculateOut()
        {
            if (input > threshold)
            {
                output = 1;
            }
            else if (input == threshold)
            {
                output = input;
            }
            else if (input < threshold)
            {
                output = 0;
            }
            return output;
        }
        public int CalculateInput(List<Neuron> prevNeurons)
        {
            input = 0;            
            for (int i = 0; i < weight.Count; i++)
            {
                input += weight[i] * prevNeurons[i].Output;
            }
            return input;
        }
        public void WeightInit(List<List<int>> matrix1, List<List<int>> matrix2, int neuronNum)
        {
            weight = new List<int>();
            for (int j = 0; j < matrix2[0].Count; j++)
            {
                int curWeight = 0;
                for (int i = 0; i < matrix1.Count; i++)
                {
                    curWeight += (2 * matrix1[i][neuronNum] - 1) * (2 * matrix2[i][j] - 1);
                }
                weight.Add(curWeight);
            }
        }
        public void InitInput(int inputVal)
        {
            input = inputVal;
        }

    }
    partial class BAM : Log
    {
        private List<int> prevIterX;
        private List<int> prevIterY;
        private List<Neuron> x_neuron;
        private List<Neuron> y_neuron;
        private List<int> output;
        public List<List<int>> lettersHistory;
        public List<List<int>> assocHistory;
        public List<int> Output
        {
            get
            {
                return output;
            }
        }
        public BAM(int inputLength, int associatorLength)
        {
            prevIterX = new List<int>();
            prevIterY = new List<int>();
            x_neuron = new List<Neuron>();
            y_neuron = new List<Neuron>();
            lettersHistory = new List<List<int>>();
            assocHistory = new List<List<int>>();
            output = new List<int>();

            for (int i = 0; i < inputLength; i++)
            {
                x_neuron.Add(new Neuron(0, inputLength));
            }
            for (int i = 0; i < associatorLength; i++)
            {
                y_neuron.Add(new Neuron(0, associatorLength));
            }
        }
        public void Teach(List<List<int>> letters, List<List<int>> vectors, ref System.Windows.Forms.ProgressBar progBar)
        {
            prevIterX = new List<int>();
            prevIterY = new List<int>();
            WriteToLogString("========FIRST LAYER WEIGHTS========");
            WriteToLogHead(vectors[0].Count, "w", 5);
            for (int i = 0; i < x_neuron.Count; i++)
            {
                progBar.Maximum++;
                progBar.PerformStep();
                x_neuron[i].WeightInit(letters, vectors, i);
                WriteToLog(x_neuron[i].Weight.ToArray(), "X", i);
                x_neuron[i].InitInput(0);
                x_neuron[i].CalculateOut();
                prevIterX.Add(x_neuron[i].Output);
            }
            WriteToLogString("========SECOND LAYER WEIGHTS========");
            WriteToLogHead(letters[0].Count, "w", 5);
            for (int i = 0; i < y_neuron.Count; i++)
            {
                progBar.Maximum++;
                progBar.PerformStep();
                y_neuron[i].WeightInit(vectors, letters, i);
                WriteToLog(y_neuron[i].Weight.ToArray(), "Y", i);
                y_neuron[i].InitInput(0);
                y_neuron[i].CalculateOut();
                prevIterY.Add(y_neuron[i].Output);
            }
            
        }
        private void InitNeurons(ref List<Neuron> neuron, ref List<int> prevIter, List<int> vector)
        {
            for (int i = 0; i < neuron.Count; i++)
            {
                prevIter[i] = neuron[i].Output;
                neuron[i].InitInput(vector[i]);
                neuron[i].CalculateOut();
            }
        }
        
        private void WorkNet(ref List<Neuron> first_neuron, ref List<int> first_prevIter, ref List<Neuron> second_neuron, ref List<int> second_prevIter, ref List<List<int>> first_history, ref List<List<int>> second_history, ref System.Windows.Forms.ProgressBar progBar) 
        {
            progBar.Maximum++;
            progBar.PerformStep();
            List<int> cur1 = new List<int>();
            List<int> temp1 = new List<int>();
            for (int i = 0; i < second_neuron.Count; i++)
            {
                second_prevIter[i] = second_neuron[i].Output;
                cur1.Add(second_neuron[i].Output);
                temp1.Add(second_neuron[i].CalculateInput(first_neuron));
                second_neuron[i].CalculateOut();
            }
            WriteToLog(temp1.ToArray(), "1inp", -1);
            WriteToLog(second_prevIter.ToArray(), "prev", -1);
            WriteToLog(second_neuron, "now", -1, "output");
            
            second_history.Add(cur1);
            List<int> cur2 = new List<int>();
            List<int> temp2 = new List<int>();
            for (int i = 0; i < first_neuron.Count; i++)
            {
                first_prevIter[i] = first_neuron[i].Output;
                cur2.Add(first_neuron[i].Output);
                temp2.Add(first_neuron[i].CalculateInput(second_neuron));
                first_neuron[i].CalculateOut();
            }
            WriteToLog(temp2.ToArray(), "2inp", -1);
            WriteToLog(first_prevIter.ToArray(), "prev", -1);
            WriteToLog(first_neuron, "now", -1, "output");
            
            first_history.Add(cur2);
            if (!Stop())
            {
                WorkNet(ref first_neuron, ref first_prevIter, ref second_neuron, ref second_prevIter, ref first_history, ref second_history, ref progBar);
            }
        }
        public void Recognize(List<int> vector, bool isLetter, ref System.Windows.Forms.ProgressBar progBar)
        {
            lettersHistory = new List<List<int>>();
            assocHistory = new List<List<int>>();
            WriteToLogString("=======RECOGNIZING=======");
            if (isLetter)
            {
                InitNeurons(ref x_neuron, ref prevIterX, vector);
                WorkNet(ref x_neuron, ref prevIterX, ref y_neuron, ref prevIterY, ref lettersHistory, ref assocHistory, ref progBar);
            }
            else
            {
                InitNeurons(ref y_neuron, ref prevIterY, vector);
                WorkNet(ref y_neuron, ref prevIterY, ref x_neuron, ref prevIterX, ref assocHistory, ref lettersHistory, ref progBar);
            }
        }
        private bool Stop()
        {
            for (int i = 0; i < x_neuron.Count; i++)
            {
                if (x_neuron[i].Output != prevIterX[i])
                {
                    return false;
                }
            }
            for (int i = 0; i < y_neuron.Count; i++)
            {
                if (y_neuron[i].Output != prevIterY[i])
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
        public void MakeVisualLet(int num, int val)
        {
            if (val == 1)
                standartRecognized[num].CheckState = System.Windows.Forms.CheckState.Indeterminate;
            else
                standartRecognized[num].CheckState = System.Windows.Forms.CheckState.Unchecked;
        }
        public void MakeVisualAssoc(int num, int val)
        {
            if (val == 1)
                standartAssocRecognized[num].CheckState = System.Windows.Forms.CheckState.Indeterminate;
            else
                standartAssocRecognized[num].CheckState = System.Windows.Forms.CheckState.Unchecked;
        }
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
            this.showCheck = new System.Windows.Forms.Button();
            //this.neuronVisual = new System.Windows.Forms.Label[Constants.maxInputCount];
            this.recognizeByLetter = new System.Windows.Forms.RadioButton();
            this.recognizeByAssociation = new System.Windows.Forms.RadioButton();
            //this.quantWork = new System.Windows.Forms.RadioButton();
            this.signs = new System.Windows.Forms.Label[Constants.lettersCount];
            this.toRecSign = new System.Windows.Forms.Label();
            this.recSign = new System.Windows.Forms.Label();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                System.Windows.Forms.CheckBox[] letter = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
                System.Windows.Forms.CheckBox[] assoc = new System.Windows.Forms.CheckBox[Constants.maxInputCount / 4];
                signs[i] = new System.Windows.Forms.Label();
                for (int j = 0; j < Constants.maxInputCount; j++)
                {
                    letter[j] = new System.Windows.Forms.CheckBox();
                }
                for (int j = 0; j < Constants.maxInputCount / 4; j++)
                {
                    assoc[j] = new System.Windows.Forms.CheckBox();
                }
                this.letters.Add(letter);
                this.associat.Add(assoc);
            }
            this.standart = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
            this.standartAssoc = new System.Windows.Forms.CheckBox[Constants.maxInputCount / 4];
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.standart[i] = new System.Windows.Forms.CheckBox();
            }
            for (int i = 0; i < Constants.maxInputCount / 4; i++)
            {
                this.standartAssoc[i] = new System.Windows.Forms.CheckBox();
            }



            this.standartRecognized = new System.Windows.Forms.CheckBox[Constants.maxInputCount];
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                this.standartRecognized[i] = new System.Windows.Forms.CheckBox();
            }
            this.standartAssocRecognized = new System.Windows.Forms.CheckBox[Constants.maxInputCount / 4];
            for (int i = 0; i < Constants.maxInputCount / 4; i++)
            {
                this.standartAssocRecognized[i] = new System.Windows.Forms.CheckBox();
            }

           // this.visLabel = new System.Windows.Forms.Label();

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
            //associat
            //
            for (int i = 0; i < Constants.maxInputCount / 4; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    associat[j][i].AutoSize = true;
                    associat[j][i].Location = new System.Drawing.Point(13, 6);
                    associat[j][i].Size = new System.Drawing.Size(80, 17);
                    associat[j][i].ThreeState = true;
                    associat[j][i].UseVisualStyleBackColor = true;
                }

                this.standartAssoc[i].AutoSize = true;
                this.standartAssoc[i].Location = new System.Drawing.Point(13, 6);
                this.standartAssoc[i].Size = new System.Drawing.Size(80, 17);
                this.standartAssoc[i].ThreeState = true;
                this.standartAssoc[i].UseVisualStyleBackColor = true;


                this.standartAssocRecognized[i].AutoSize = true;
                this.standartAssocRecognized[i].Location = new System.Drawing.Point(13, 6);
                this.standartAssocRecognized[i].Size = new System.Drawing.Size(80, 17);
                this.standartAssocRecognized[i].ThreeState = true;
                this.standartAssocRecognized[i].UseVisualStyleBackColor = true;
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
                this.signs[i].Text = "Sing №" + (i + 1);
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
            /*
            this.visLabel.AutoSize = true;
            this.visLabel.Location = new System.Drawing.Point(5, 5);
            this.visLabel.Name = "toRecSign";
            this.visLabel.Size = new System.Drawing.Size(32, 13);
            this.visLabel.TabIndex = 5;
            this.visLabel.Text = "Neurons activity";
            */

            // 
            // allWork
            // 
            this.recognizeByLetter.AutoSize = true;
            this.recognizeByLetter.Location = new System.Drawing.Point(130, 30);
            this.recognizeByLetter.Name = "recognizeByLetter";
            this.recognizeByLetter.Size = new System.Drawing.Size(31, 17);
            this.recognizeByLetter.TabIndex = 0;
            this.recognizeByLetter.TabStop = true;
            this.recognizeByLetter.Text = "recognize by large letter";
            this.recognizeByLetter.UseVisualStyleBackColor = true;

            // 
            // recognizeByAssociation
            // 
            this.recognizeByAssociation.AutoSize = true;
            this.recognizeByAssociation.Location = new System.Drawing.Point(130, 45);
            this.recognizeByAssociation.Name = "recognizeByAssociation";
            this.recognizeByAssociation.Size = new System.Drawing.Size(31, 17);
            this.recognizeByAssociation.TabIndex = 0;
            this.recognizeByAssociation.TabStop = true;
            this.recognizeByAssociation.Text = "recognize by small letter";
            this.recognizeByAssociation.UseVisualStyleBackColor = true;

            // 
            // quantWork
            // 
            /*
            this.quantWork.AutoSize = true;
            this.quantWork.Location = new System.Drawing.Point(130, 60);
            this.quantWork.Name = "quantWork";
            this.quantWork.Size = new System.Drawing.Size(31, 17);
            this.quantWork.TabIndex = 0;
            this.quantWork.TabStop = true;
            this.quantWork.Text = "quantity of neurons work";
            this.quantWork.UseVisualStyleBackColor = true;
            */

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
            this.pictureBox1.Image = global::nk_lab_6.Properties.Resources.Capture;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 53);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
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
            this.Controls.Add(this.recognizeByLetter);
            this.Controls.Add(this.recognizeByAssociation);
            //this.Controls.Add(this.quantWork);
            //this.Controls.Add(this.visLabel);

            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                for (int j = 0; j < letters.Count; j++)
                {
                    this.Controls.Add(letters[j][i]);
                }
                this.Controls.Add(this.standart[i]);
                this.Controls.Add(this.standartRecognized[i]);
                //this.Controls.Add(this.neuronVisual[i]);
            }
            for (int i = 0; i < Constants.maxInputCount / 4; i++ )
            {
                for (int j = 0; j < associat.Count; j++)
                {
                    this.Controls.Add(associat[j][i]);
                }
                this.Controls.Add(this.standartAssoc[i]);
                this.Controls.Add(this.standartAssocRecognized[i]);
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
            this.Text = "Bidirectional associative memory";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        protected void MakeLetters(int letterHeight, int letterWidth)
        {
            this.loading.Visible = true;
            this.loadLabel.Visible = true;
            for (int i = 0; i < Constants.maxInputCount; i++)
            {
                //this.neuronVisual[i].Location = new System.Drawing.Point(5, 5);
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
            for (int i = 0; i < Constants.maxInputCount / 4; i++)
            {
                //this.neuronVisual[i].Location = new System.Drawing.Point(5, 5);
                for (int j = 0; j < associat.Count; j++)
                {
                    associat[j][i].Location = new System.Drawing.Point(5, 5);
                    associat[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standartAssoc[i].Location = new System.Drawing.Point(5, 5);
                    standartAssocRecognized[i].Location = new System.Drawing.Point(5, 5);
                    standartAssoc[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    standartAssocRecognized[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
            }
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i].Location = new System.Drawing.Point(5, 5);
            }
            toRecSign.Location = new System.Drawing.Point(5, 5);
            recSign.Location = new System.Drawing.Point(5, 5);
            //visLabel.Location = new System.Drawing.Point(5, 5);
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
            int offset = (3 * letterWidth / 2) * step + 2 * step;
            
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
                //this.neuronVisual[i].Location = new System.Drawing.Point(xx + labelOffset * 35, yy);
                for (int j = 0; j < letters.Count; j++)
                {
                    letters[j][i].Location = new System.Drawing.Point(x + j * offset, y);
                }
                this.standart[i].Location = new System.Drawing.Point(x + letters.Count * offset + step, y);
                this.standartRecognized[i].Location = new System.Drawing.Point(x + (letters.Count + 1) * offset + step, y);
            }
            
            int widthAssoc = letterWidth / 2;
            int countAssoc = count / 4;
            int yAssoc = 150 + 15 * letterHeight / 2;
            int xConstAssoc = letters[0][count - 1].Location.X + step;
            int xAssoc = xConstAssoc;
            for (int i = 0; i < countAssoc; i++)
            {
                //labelOffset++;
                if (i + 1 > widthAssoc)
                {
                    //labelOffset = 1;
                    xAssoc = xConstAssoc;
                    yAssoc += step;
                    widthAssoc += letterWidth / 2;
                    //xx = labelX;
                    //yy += step;

                }
                xAssoc += step;
                //this.neuronVisual[i].Location = new System.Drawing.Point(xx + labelOffset * 35, yy);
                for (int j = 0; j < associat.Count; j++)
                {
                    associat[j][i].Location = new System.Drawing.Point(xAssoc + j * offset, yAssoc);
                }
                this.standartAssoc[i].Location = new System.Drawing.Point(xAssoc + letters.Count * offset + step, yAssoc);
                this.standartAssocRecognized[i].Location = new System.Drawing.Point(xAssoc + (letters.Count + 1) * offset + step, yAssoc);
            }

            //visLabel.Location = new System.Drawing.Point(neuronVisual[0].Location.X, neuronVisual[0].Location.Y - 20);
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
        protected List<System.Windows.Forms.CheckBox[]> associat = new List<System.Windows.Forms.CheckBox[]>();
        protected System.Windows.Forms.CheckBox[] standart;
        protected System.Windows.Forms.CheckBox[] standartRecognized;
        protected System.Windows.Forms.CheckBox[] standartAssoc;
        protected System.Windows.Forms.CheckBox[] standartAssocRecognized;
        protected System.Windows.Forms.Button showCheck;
        protected System.Windows.Forms.Button test;
        protected System.Windows.Forms.Button teach;
        protected System.Windows.Forms.Button recognize;
        protected System.Windows.Forms.Button about;
        //private System.Windows.Forms.Label[] neuronVisual;
        protected System.Windows.Forms.RadioButton recognizeByLetter;
        protected System.Windows.Forms.RadioButton recognizeByAssociation;
        //protected System.Windows.Forms.RadioButton quantWork;
        protected int count;

        protected System.Windows.Forms.Label[] signs;
        protected System.Windows.Forms.Label toRecSign;
        protected System.Windows.Forms.Label recSign;
        //protected System.Windows.Forms.Label visLabel;

        private System.Windows.Forms.ProgressBar loading;
        protected System.Windows.Forms.Label loadLabel;
    }
}

