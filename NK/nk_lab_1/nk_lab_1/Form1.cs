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
        private void showCheck_Click(object sender, EventArgs e)
        {
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
        private void teach_Click(object sender, EventArgs e)
        {
            File.WriteAllText("log.txt", "");
            int epochCount = 0;
            if (maxEpochCountInit.Text != "")
                epochCount = Convert.ToInt32(maxEpochCountInit.Text) * Constants.lettersCount;
            else
                System.Windows.Forms.MessageBox.Show("Max epoch count field is empty");
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
            if (neuronNet.Teach(standartLetters, Constants.outStandart))
                System.Windows.Forms.MessageBox.Show("Teach successful. Epoch count: " + (neuronNet.Epoch/4).ToString() + ". View 'log.txt' for more information.");
            else
                System.Windows.Forms.MessageBox.Show("Teach imposible");
            
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
            neuronNet.Recognize(inLetter, Constants.outStandart);
        }
        private void about_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}
