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

namespace nk_lab_3
{
    public partial class Form1 : Form
    {
        private int inputSize;
        decimal k;
        HebbNet hebbNet;
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
                k = Convert.ToDecimal(coeficientValue.Text);
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
            hebbNet = new HebbNet(standartLetters.Count, inputSize, k);
            hebbNet.WriteToLogString("===============INITIALIZATION===============");
            hebbNet.InitNet(standartLetters);
            MessageBox.Show("Initialisation successful");
            MessageBox.Show("View 'log.txt' for more information");
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
            int sgnNum = hebbNet.Recognize(inLetter);
            epsilonValue.Text = String.Format("{0:0.000}", hebbNet.GetEpsilon());
            if (sgnNum != -1)
                MessageBox.Show("Sign №" + (sgnNum + 1).ToString() + ". Iterations count " + hebbNet.GetIterCount().ToString());
            else
                MessageBox.Show("Can't recognize");
            MessageBox.Show("View 'log.txt' for more information");
           
        }
        private void about_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
