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
            if (heightValue.Text != "" && widthValue.Text != "" && coeficientValue.Text != "" /*&& stepValue.Text != ""*/)
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
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            
            int[] inLetter = new int[count];
            for (int j = 0; j < inLetter.Length; j++)
            {

                if (standart[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standart[j].CheckState == System.Windows.Forms.CheckState.Checked)
                    inLetter[j] = 1;
                else
                    inLetter[j] = -1;
            }
            hopfieldNet.InitNeurons(inLetter);
            hopfieldNet.Recognize();
            makeVisDel = new MakeVisualDel(MakeVisual);
            t1 = new Thread(StartAnim); // создаем поток  
            t1.IsBackground = true; // задаем фоновый режым  
            t1.Priority = ThreadPriority.Lowest; // указываем свмый низкий приоритет  
            t1.Start(); // стартуем 
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
                System.Threading.Thread.Sleep(25);
                ResetVisual();
            }
            
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
