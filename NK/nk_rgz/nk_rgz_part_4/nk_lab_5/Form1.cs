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

namespace nk_lab_5
{
    public partial class Form1 : Form
    {
        KohonenNet kohNet;
        int size;
        Log log = new Log(); 
        List<List<decimal>> weightsInit = new List<List<decimal>>();
        bool teached;
        public Form1()
        {
            InitializeComponent();
            File.WriteAllText("log.txt", "");
        }
        private List<List<int>> Inverse(int vecNum, int bitNum, List<List<int>> vectors)
        {
            if (vectors[vecNum][bitNum] == 1)
                vectors[vecNum][bitNum] = 0;
            else
                vectors[vecNum][bitNum] = 1;
            return vectors;
        }
        private void showCheck_Click(object sender, EventArgs e)
        {
            teached = false;
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (heightValue.Text != "" && widthValue.Text != "" && rValue.Text != "" && kValue.Text != "" && alphaValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                size = letterHeight * letterWidth;
                MakeLetters(letterHeight, letterWidth);
                File.WriteAllText("log.txt", "");
                decimal alphaVal = Convert.ToDecimal(alphaValue.Text);
                int radiusVal = Convert.ToInt32(rValue.Text);
                decimal coeff = Convert.ToDecimal(kValue.Text);
                log.WriteToLogString("α = " + alphaValue.Text + "; R = " + rValue.Text + "; k = " + kValue.Text);
                kohNet = new KohonenNet(size, alphaVal, radiusVal, coeff);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void teach_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch teachTime = new System.Diagnostics.Stopwatch();
            teached = true;
            List<List<int>> standartLetters = new List<List<int>>();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                List<int> letter = new List<int>();
                for (int j = 0; j < size; j++)
                {
                    if (letters[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || letters[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                        letter.Add(1);
                    else
                        letter.Add(0);
                }
                //log.WriteToLog(letter.ToArray(), "vec", -1);
                standartLetters.Add(letter);
            }
            kohNet.InitAxons(standartLetters, ref teachTime);
            if (kohNet.Teach(standartLetters, ref loading, ref teachTime))
            {
                MessageBox.Show("Create successful by " + kohNet.Iteration.ToString() + " iterations. Teach time: " + String.Format("{0:0.0000}", ((decimal)teachTime.ElapsedTicks * 1000 / System.Diagnostics.Stopwatch.Frequency)) + "mS. View 'log.txt' for more information");
            }
        }
        private void test_Click(object sender, EventArgs e)
        {
            teached = false;
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            int[] letter1 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter2 = { -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter3 = { -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1 };
            int[] letter4 = { -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter5 = { -1, -1, -1, 1, 1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, -1, -1 };
            List<int[]> vectors = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
            vectors.Add(letter3);
            vectors.Add(letter4);
            vectors.Add(letter5);
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
            alphaValue.Text = "0.8";
            rValue.Text = "0";
            kValue.Text = "0.6";
            int letterHeight = 14;
            int letterWidth = 10;
            size = letterHeight * letterWidth;
            MakeLetters(letterHeight, letterWidth);
            File.WriteAllText("log.txt", "");
            decimal alphaVal = Convert.ToDecimal(alphaValue.Text);
            int radiusVal = Convert.ToInt32(rValue.Text);
            decimal coeff = Convert.ToDecimal(kValue.Text);
            log.WriteToLogString("α = " + alphaValue.Text + "; R = " + rValue.Text + "; k = " + kValue.Text);
            kohNet = new KohonenNet(size, alphaVal, radiusVal, coeff);
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch recTime = new System.Diagnostics.Stopwatch();
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
                log.WriteToLogString("======RECOGNIZING=======");
                MessageBox.Show("Sign №" + (kohNet.Recognize(inLetter, ref recTime) + 1) + " Recognize time: " + String.Format("{0:0.0000}", ((decimal)recTime.ElapsedTicks * 1000 / System.Diagnostics.Stopwatch.Frequency)) + " mS");
            }
            else
            {
                MessageBox.Show("!!Press 'Teach' first!!");
            }
        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}