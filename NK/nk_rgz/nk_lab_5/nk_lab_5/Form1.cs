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
        Random rand = new Random();
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
                for (int j = 0; j < Constants.lettersCount; j++)
                {
                    List<decimal> curW = new List<decimal>();
                    for (int i = 0; i < size; i++)
                    {
                        curW.Add((decimal)rand.Next(1, 10) / 10);
                    }
                    weightsInit.Add(curW);
                }
                kohNet.InitAxons(weightsInit);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void teach_Click(object sender, EventArgs e)
        {
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
                log.WriteToLog(letter.ToArray(), "vec", -1);
                standartLetters.Add(letter);
            }
            if (kohNet.Teach(standartLetters))
            {
                MessageBox.Show("Create successful by " + kohNet.Iteration.ToString() + " iterations. View 'log.txt' for more information");
            }
        }

        private void recognize_Click(object sender, EventArgs e)
        {
            
        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}