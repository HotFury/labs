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
namespace nk_lab_7
{
    public partial class Form1 : Form
    {
        private int inputSize;
        private bool teached = false;
        private ART_1 art_1;
        public Form1()
        {
            InitializeComponent();
        }
        private void showCheck_Click(object sender, EventArgs e)
        {
            teached = false;
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (heightValue.Text != "" && widthValue.Text != "" && pValue.Text != "" && LValue.Text != "" && maxEpochValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                decimal p = Convert.ToDecimal(pValue.Text);
                int l = Convert.ToInt32(LValue.Text);
                int maxEpochCount = Convert.ToInt32(maxEpochValue.Text);
                inputSize = letterHeight * letterWidth;
                MakeLetters(letterHeight, letterWidth);
                File.WriteAllText("log.txt", "");
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void test_Click(object sender, EventArgs e)
        {
            pValue.Text = "0.9";
            LValue.Text = "2";
            maxEpochValue.Text = "10";
            teached = false;
            int letterHeight = 14;
            int letterWidth = 10;
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            decimal p = Convert.ToDecimal(pValue.Text);
            int l = Convert.ToInt32(LValue.Text);
            int maxEpochCount = Convert.ToInt32(maxEpochValue.Text);
            inputSize = letterHeight * letterWidth;
            MakeLetters(letterHeight, letterWidth);
            File.WriteAllText("log.txt", "");
            int[] letter1 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter2 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter3 = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] letter4 = { -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter5 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1 };
            int[] letter6 = { 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            List<int[]> vectors = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
            vectors.Add(letter3);
            vectors.Add(letter4);
            vectors.Add(letter5);
            vectors.Add(letter6);
            for (int j = 0; j < vectors.Count; j++)
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
            inputSize = letterHeight * letterWidth;

            
            File.WriteAllText("log.txt", "");
        }
        private void teach_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (pValue.Text != "" && LValue.Text != "" && maxEpochValue.Text != "")
            {
                decimal p = Convert.ToDecimal(pValue.Text);
                int l = Convert.ToInt32(LValue.Text);
                int maxEpochCount = Convert.ToInt32(maxEpochValue.Text);
                File.WriteAllText("log.txt", "");
                teached = true;
                File.WriteAllText("log.txt", "");
                List<List<int>> standartLetters = new List<List<int>>();
                for (int i = 0; i < Constants.lettersCount; i++)
                {
                    List<int> letter = new List<int>();
                    for (int j = 0; j < inputSize; j++)
                    {
                        if (letters[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || letters[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                            letter.Add(1);
                        else
                            letter.Add(0);
                    }
                    standartLetters.Add(letter);
                }
                art_1 = new ART_1();
                art_1.InitNet(inputSize, p, l, maxEpochCount);
                art_1.Teach(standartLetters, ref loading);
                MessageBox.Show("Teach successful. Epoch count: " + art_1.EpochCount.ToString());
                MessageBox.Show("View 'log.txt' for more information");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
            
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            if (teached)
            {
                List<int> inLetter = new List<int>();
                for (int j = 0; j < count; j++)
                {

                    if (standart[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standart[j].CheckState == System.Windows.Forms.CheckState.Checked)
                        inLetter.Add(1);
                    else
                        inLetter.Add(0);

                }
                int sgnNum = art_1.Recognize(inLetter);
                MessageBox.Show("Sign №" + (sgnNum + 1).ToString());
                /*int sgnNum = hebbNet.Recognize(inLetter);
                epsilonValue.Text = String.Format("{0:0.000}", hebbNet.GetEpsilon());
                if (sgnNum != -1)
                    MessageBox.Show("Sign №" + (sgnNum + 1).ToString());
                else
                    MessageBox.Show("Can't recognize");
                MessageBox.Show("View 'log.txt' for more information");
                 * */
            }
            else
            {
                MessageBox.Show("!!Press 'Teach' first!!");
            }
           
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
