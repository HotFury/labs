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

namespace nk_lab_1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private NeuronNet neuronNet;
        private int inputSize;
        private bool teached = false;
        private void showCheck_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            teached = false;
            if (heightValue.Text != "" && widthValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                MakeLetters(letterHeight, letterWidth);
                inputSize = letterHeight * letterWidth + 1;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill both fields: 'height' and 'width'");
            }

        }
        private void test_Click(object sender, EventArgs e)
        {
            maxEpochCountInit.Text = "50";
            loading.Value = 0;
            teached = false;
            int letterHeight = 14;
            int letterWidth = 10;
            inputSize = letterHeight * letterWidth + 1;
            MakeLetters(letterHeight, letterWidth);
            File.WriteAllText("log.txt", "");
            int[] letter1 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter2 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter3 = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] letter4 = { -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, 1, 1 };
            List<int[]> vectors = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
            vectors.Add(letter3);
            vectors.Add(letter4);
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
        }
        private void teach_Click(object sender, EventArgs e)
        {
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            teached = true;
            File.WriteAllText("log.txt", "");
            int epochCount = 0;
            if (maxEpochCountInit.Text != "")
            {
                epochCount = Convert.ToInt32(maxEpochCountInit.Text) * Constants.lettersCount;
                if (fourNeuronInit.Checked)
                    Constants.InitConstants(4, epochCount);
                else if (twoNeuronInit.Checked)
                    Constants.InitConstants(2, epochCount);
                neuronNet = new NeuronNet(inputSize);
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
                if (neuronNet.Teach(standartLetters, Constants.outStandart, ref loading))
                    System.Windows.Forms.MessageBox.Show("Teach successful. Epoch count: " + (neuronNet.Epoch / Constants.lettersCount).ToString() + ". View 'log.txt' for more information.");
                else
                    System.Windows.Forms.MessageBox.Show("Teach imposible");
            }
            else
                System.Windows.Forms.MessageBox.Show("Max epoch count field is empty");
            
            
        }
        private void recognize_Click(object sender, EventArgs e)
        {
            if (teached)
            {
                int[] inLetter = new int[count];
                for (int j = 0; j < inLetter.Length; j++)
                {

                    if (standart[j].CheckState == System.Windows.Forms.CheckState.Indeterminate || standart[j].CheckState == System.Windows.Forms.CheckState.Checked)
                        inLetter[j] = 1;
                    else
                        inLetter[j] = -1;

                }
                neuronNet.Recognize(inLetter, Constants.outStandart);
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
