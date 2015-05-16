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
        private int inputSize;
        HopfieldNet hopfieldNet;
        public Form1()
        {
            InitializeComponent();
        }
        private void showCheck_Click(object sender, EventArgs e)
        {
            if (heightValue.Text != "" && widthValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                inputSize = letterHeight * letterWidth;
                
                MakeLetters(letterHeight, letterWidth);
                File.WriteAllText("log.txt", "");
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void teach_Click(object sender, EventArgs e)
        {
            
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
            hopfieldNet.InitNet(standartLetters);
            MessageBox.Show("Teach succesfull. View 'log.txt' for more information");
        }
        private void recognize_Click(object sender, EventArgs e)
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
            for (int i = 0; i < hopfieldNet.Input.Length; i++ )
            {
                standartRecognized[i].CheckState = System.Windows.Forms.CheckState.Unchecked;
            }
            hopfieldNet.Recognize();
            makeVisDel = new MakeVisualDel(MakeVisual);
            t1 = new Thread(StartAnim); // создаем поток  
            t1.IsBackground = true; // задаем фоновый режым  
            t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
            t1.Start(); // стартуем 
            DrawLettersHistory(hopfieldNet.LettersHistory);
            MessageBox.Show("Recognize successfull!");
            
        }
        void StartAnim()
        {
            foreach(List<int> curIter in hopfieldNet.Iterations)
            {
                foreach (int curNum in curIter)
                {
                    int num = curNum;
                    Invoke(makeVisDel, num);
                }
                System.Threading.Thread.Sleep(500);
                ResetVisual();
                System.Threading.Thread.Sleep(200);
            }
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
