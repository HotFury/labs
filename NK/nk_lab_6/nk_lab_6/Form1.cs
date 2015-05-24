using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace nk_lab_6
{
    public partial class Form1 : Form
    {
        private bool teached;
        private int inputSize;
        BAM bam;

        public delegate void MakeVisualLetDel(int num, int val);
        MakeVisualLetDel makeVisLetDel;
        Thread t1;

        public delegate void MakeVisualAssocDel(int num, int val);
        MakeVisualAssocDel makeVisAssocDel;
        Thread t2;
        public Form1()
        {
            InitializeComponent();
        }
        private void showCheck_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            if (heightValue.Text != "" && widthValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                inputSize = letterHeight * letterWidth;

                MakeLetters(letterHeight, letterWidth);
                File.WriteAllText("log.txt", "");
                teached = false;
                bam = new BAM(inputSize, inputSize / 4);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void test_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            teached = false;
            int letterHeight = 14;
            int letterWidth = 10;
            inputSize = letterHeight * letterWidth;
            MakeLetters(letterHeight, letterWidth);
            File.WriteAllText("log.txt", "");
            int[] letter1 = { -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter2 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] assoc1 = { 1, -1, -1, -1, 1, 1, -1, -1, 1, -1, 1, -1, 1, -1, -1, 1, 1, -1, -1, -1, 1, -1, 1, -1, -1, 1, -1, -1, 1, -1, 1, -1, -1, -1, 1 };
            int[] assoc2 = { -1, -1, 1, -1, -1, -1, 1, -1, 1, -1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1 };
            List<int[]> vectors = new List<int[]>();
            List<int[]> assoc = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
            assoc.Add(assoc1);
            assoc.Add(assoc2);
            for (int j = 0; j < letters.Count; j++)
            {
                for (int i = 0; i < vectors[j].Length; i++)
                {
                    if (vectors[j][i] == 1)
                    {
                        letters[j][i].CheckState = System.Windows.Forms.CheckState.Indeterminate;
                    }
                    else
                    {
                        letters[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    }
                }
            }
            for (int j = 0; j < associat.Count; j++)
            {
                for (int i = 0; i < assoc[j].Length; i++)
                {
                    if (assoc[j][i] == 1)
                    {
                        associat[j][i].CheckState = System.Windows.Forms.CheckState.Indeterminate;
                    }
                    else
                    {
                        associat[j][i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                    }
                }
            }
            bam = new BAM(inputSize, inputSize / 4);
        }
        private void teach_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            teached = true;
            File.WriteAllText("log.txt", "");
            List<List<int>> standartLetters = new List<List<int>>();
            List<List<int>> standartAssoc = new List<List<int>>();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                List<int> letter = new List<int>();
                for (int j = 0; j < count; j++)
                {
                    if (letters[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || letters[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                        letter.Add(1);
                    else
                        letter.Add(0);
                }
                standartLetters.Add(letter);

                List<int> assoc = new List<int>();
                for (int j = 0; j < count / 4; j++)
                {
                    if (associat[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || associat[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                        assoc.Add(1);
                    else
                        assoc.Add(0);
                }
                standartAssoc.Add(assoc);
            }
            /*
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                
            }*/
            bam.Teach(standartLetters, standartAssoc, ref loading);
            /*hopfieldNet = new HopfieldNet(inputSize, Constants.lettersCount);
            hopfieldNet.InitNet(standartLetters, ref loading);
            Log log = new Log();
            foreach (int[] curLet in standartLetters)
                log.WriteToLog(curLet, "x", 0);*/
            MessageBox.Show("Teach succesfull. View 'log.txt' for more information");
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (teached)
            {                
                if (this.recognizeByLetter.Checked)
                {
                    List<int> inLetter = new List<int>();
                    for (int j = 0; j < count; j++)
                    {

                        if (standart[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standart[j].CheckState == System.Windows.Forms.CheckState.Checked)
                            inLetter.Add(1);
                        else
                            inLetter.Add(0);
                    }
                    bam.Recognize(inLetter, true, ref loading);
                }
                else if (this.recognizeByAssociation.Checked)
                {
                    List<int> inAssoc = new List<int>();
                    for (int j = 0; j < count / 4; j++)
                    {

                        if (standartAssoc[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standartAssoc[j].CheckState == System.Windows.Forms.CheckState.Checked)
                            inAssoc.Add(1);
                        else
                            inAssoc.Add(0);
                    }
                    bam.Recognize(inAssoc, false, ref loading);
                }
                MessageBox.Show("Calculation complete. Now look visual");
                //MessageBox.Show("Recognize succesfull");
                /*hopfieldNet.InitNeurons(inLetter);
                for (int i = 0; i < hopfieldNet.Input.Length; i++)
                {
                    standartRecognized[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
                hopfieldNet.Recognize(ref loading);
                
                makeVisDel = new MakeVisualDel(MakeVisual);
                t1 = new Thread(StartAnim); // создаем поток  
                t1.IsBackground = true; // задаем фоновый режым  
                t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
                t1.Start(); // стартуем
                */
                makeVisLetDel = new MakeVisualLetDel(MakeVisualLet);
                t1 = new Thread(StartAnimLet); // создаем поток  
                t1.IsBackground = true; // задаем фоновый режым  
                t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
                t1.Start(); // стартуем*/

                makeVisAssocDel = new MakeVisualAssocDel(MakeVisualAssoc);
                t2 = new Thread(StartAnimAssoc); // создаем поток  
                t2.IsBackground = true; // задаем фоновый режым  
                t2.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
                t2.Start(); // стартуем*/
            }
            else
            {
                MessageBox.Show("!!click 'Teach' first!!");
            }
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
        void StartAnimLet()
        {
            int sleepTime = 0;
            sleepTime = 300;
            foreach (List<int> curLetter in bam.lettersHistory)
            {
                for (int i = 0; i < curLetter.Count; i++)
                {
                    Invoke(makeVisLetDel, i, curLetter[i]);
                }
                System.Threading.Thread.Sleep(sleepTime);
            }
        }
        void StartAnimAssoc()
        {
            int sleepTime = 0;
            sleepTime = 300;
            foreach (List<int> curLetter in bam.assocHistory)
            {
                for (int i = 0; i < curLetter.Count; i++)
                {
                    Invoke(makeVisAssocDel, i, curLetter[i]);
                }
                System.Threading.Thread.Sleep(sleepTime);
            }
        }
    }
}
