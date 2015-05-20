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

namespace nk_lab_4
{
    public partial class Form1 : Form
    {
        public delegate void MakeVisualDel(int num);
        MakeVisualDel makeVisDel;
        Thread t1;

        public delegate void MakeVisualLetDel(int num, int val);
        MakeVisualLetDel makeVisLetDel;
        Thread t2;

        private int inputSize;
        HopfieldNet hopfieldNet;
        private bool teached;
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
            int[] letter3 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1 };
            List<int[]> vectors = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
            vectors.Add(letter3);
            for (int j = 0; j < letters.Count; j++ )
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
        
        }
        private void teach_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            teached = true;
            File.WriteAllText("log.txt", "");
            List<int[]> standartLetters = new List<int[]>();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                int[] letter = new int[count];
                for (int j = 0; j < letter.Length; j++)
                {
                    if (letters[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || letters[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                        letter[j] = 1;
                    else
                        letter[j] = -1;
                }
                standartLetters.Add(letter);
            }
            hopfieldNet = new HopfieldNet(inputSize, Constants.lettersCount);
            hopfieldNet.InitNet(standartLetters, ref loading);
            Log log = new Log();
            foreach (int[] curLet in standartLetters)
                log.WriteToLog(curLet, "x", 0);
            MessageBox.Show("Teach succesfull. View 'log.txt' for more information");
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (teached)
            {
                hopfieldNet.ReserIterations_LettersHistory();
                if (allWork.Checked)
                {
                    hopfieldNet.InitMode("allWork");
                }
                else if (randWork.Checked)
                {
                    hopfieldNet.InitMode("randWork");
                }
                else if (quantWork.Checked)
                {
                    hopfieldNet.InitMode("quantWork");
                }
                int[] inLetter = new int[count];
                for (int j = 0; j < inLetter.Length; j++)
                {

                    if (standart[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standart[j].CheckState == System.Windows.Forms.CheckState.Checked)
                        inLetter[j] = 1;
                    else
                        inLetter[j] = -1;
                }
                hopfieldNet.InitNeurons(inLetter);
                for (int i = 0; i < hopfieldNet.Input.Length; i++)
                {
                    standartRecognized[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
                hopfieldNet.Recognize(ref loading);
                MessageBox.Show("Calculation complete. Now look visual");
                makeVisDel = new MakeVisualDel(MakeVisual);
                t1 = new Thread(StartAnim); // создаем поток  
                t1.IsBackground = true; // задаем фоновый режым  
                t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
                t1.Start(); // стартуем

                makeVisLetDel = new MakeVisualLetDel(MakeVisualLet);
                t2 = new Thread(StartAnimLet); // создаем поток  
                t2.IsBackground = true; // задаем фоновый режым  
                t2.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
                t2.Start(); // стартуем
            }
            else
            {
                MessageBox.Show("!!click 'Teach' first!!");
            }
        }
        void StartAnim()
        {
            int sleepTime = 0;
            if (randWork.Checked)
            {
                sleepTime = 50;
            }
            else if (allWork.Checked || quantWork.Checked)
            {
                sleepTime = 150;
            }
            foreach(List<int> curIter in hopfieldNet.Iterations)
            {
                foreach (int curNum in curIter)
                {
                    int num = curNum;
                    Invoke(makeVisDel, num);
                }
                System.Threading.Thread.Sleep(sleepTime);
                ResetVisual();
                System.Threading.Thread.Sleep(sleepTime);
            }
            MessageBox.Show("Recognize successfull!");
        }

        void StartAnimLet()
        {
            int sleepTime = 0;
            if (randWork.Checked)
            {
                sleepTime = 100;
            }
            else if (allWork.Checked || quantWork.Checked)
            {
                sleepTime = 300;
            }
            foreach(List<int> curLetter in hopfieldNet.LettersHistory)
            {
                for (int i = 0; i < curLetter.Count; i++ )
                {
                    Invoke(makeVisLetDel, i, curLetter[i]);
                }
                System.Threading.Thread.Sleep(sleepTime);
            }
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
