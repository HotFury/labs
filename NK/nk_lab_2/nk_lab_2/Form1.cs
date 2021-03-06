﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace nk_lab_2
{
    public partial class Form1 : Form
    {
        private int inputSize;
        private List<decimal[]> associatorsInitWeight;
        private decimal[] respondersInitWeight;
        private Perceptron perceptron;
        private int associatorCount;
        private decimal[] initialWeights;
        private bool teached = false;
        private void RandWeights()
        {
            associatorsInitWeight = new List<decimal[]>();
            respondersInitWeight = new decimal[associatorCount];
            Random rand = new Random();
            for (int i = 0; i < associatorCount; i++)
            {
                decimal[] weight = new decimal[inputSize];
                for (int j = 0; j < weight.Length; j++)
                {
                    weight[j] = (decimal)rand.Next(1, 9) / 10;
                }
                associatorsInitWeight.Add(weight);
                respondersInitWeight[i] = (decimal)rand.Next(1, 9) / 10;
            }
            
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void showCheck_Click(object sender, EventArgs e)
        {
            teached = false;
            if (heightValue.Text != "" && widthValue.Text != "" && associatorCountValue.Text != "" && stepValue.Text != "")
            {
                int letterHeight = Convert.ToInt32(heightValue.Text);
                int letterWidth = Convert.ToInt32(widthValue.Text);
                associatorCount = Convert.ToInt32(associatorCountValue.Text);
                inputSize = letterHeight * letterWidth;
                perceptron = new Perceptron(inputSize, associatorCount);
                initialWeights = new decimal[associatorCount];
                perceptron.Step = Convert.ToDecimal(stepValue.Text.Replace(',','.'));
                MakeLetters(letterHeight, letterWidth);
                File.WriteAllText("log.txt", "");
                perceptron.WriteToLogString("=======INITIALIZATION=======");
                RandWeights();
                perceptron.InitWeights(associatorsInitWeight,respondersInitWeight);
                initialWeights = perceptron.Responder.SaveWeight();
                perceptron.WriteToLogHead(perceptron.Associator[0].Weight, "S",5);
                for (int i = 0; i < perceptron.Associator.Length; i++)
                {
                    perceptron.WriteToLog(perceptron.Associator[i].Weight, "A", i);
                }
                perceptron.WriteToLogHead(perceptron.Responder.Weight, "A",5);
                perceptron.WriteToLog(perceptron.Responder.Weight, "R", 0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Fill all fields");
            }
        }
        private void test_Click(object sender, EventArgs e)
        {
            teached = false;
            associatorCountValue.Text = "20";
            stepValue.Text = "0.01";
            loading.Value = 0;
            teached = false;
            int letterHeight = 14;
            int letterWidth = 10;
            inputSize = letterHeight * letterWidth;
            MakeLetters(letterHeight, letterWidth);
            int[] letter1 = { -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, 1, 1, -1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, 1, 1 };
            int[] letter2 = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            List<int[]> vectors = new List<int[]>();
            vectors.Add(letter1);
            vectors.Add(letter2);
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
            associatorCount = Convert.ToInt32(associatorCountValue.Text);
            inputSize = letterHeight * letterWidth;
            perceptron = new Perceptron(inputSize, associatorCount);
            initialWeights = new decimal[associatorCount];
            perceptron.Step = Convert.ToDecimal(stepValue.Text);
            File.WriteAllText("log.txt", "");
            perceptron.WriteToLogString("=======INITIALIZATION=======");
            RandWeights();
            perceptron.InitWeights(associatorsInitWeight, respondersInitWeight);
            initialWeights = perceptron.Responder.SaveWeight();
            perceptron.WriteToLogHead(perceptron.Associator[0].Weight, "S", 5);
            for (int i = 0; i < perceptron.Associator.Length; i++)
            {
                perceptron.WriteToLog(perceptron.Associator[i].Weight, "A", i);
            }
            perceptron.WriteToLogHead(perceptron.Responder.Weight, "A", 5);
            perceptron.WriteToLog(perceptron.Responder.Weight, "R", 0);
        }
        private void teach_Click(object sender, EventArgs e)
        {
            teached = true;
            loading.Value = 0;
            loading.Maximum = 1;
            loading.Step = 2;
            if (alphaSystem.Checked)
            {
                Constants.InitConstants(alphaSystem.Name);
                perceptron.WriteToLogString("============================================");
                perceptron.WriteToLogString("==================alpha System==============");
                perceptron.WriteToLogString("============================================");
            }
            else if (gammaSystem.Checked)
            {
                Constants.InitConstants(gammaSystem.Name);
                perceptron.WriteToLogString("============================================");
                perceptron.WriteToLogString("==================gamma System==============");
                perceptron.WriteToLogString("============================================");
            }

            List<int[]> standartLetters = new List<int[]>();
            for (int i = 0; i < Constants.lettersCount; i++)
            {
                int[] letter = new int[count];
                for (int j = 0; j < letter.Length; j++)
                {
                    if (letters[i][j].CheckState == System.Windows.Forms.CheckState.Indeterminate || letters[i][j].CheckState == System.Windows.Forms.CheckState.Checked)
                        letter[j] = 1;
                    else
                        letter[j] = 0;
                }
                standartLetters.Add(letter);
            }
            perceptron.WriteToLogHead(perceptron.Responder.Weight, "Ua", 5);
            for (int i = 0; i < standartLetters.Count; i++)
            {
                perceptron.InitSensor(standartLetters[i]);
                for (int j = 0; j < perceptron.Associator.Length; j++)
                {
                    perceptron.Associator[j].InitInput(perceptron.Sensor);

                }
                perceptron.WriteToLog(perceptron.Associator, "sign", i);
            }
            if (alphaSystem.Checked)
            {
                perceptron.Responder.LoadWeigth(initialWeights);
                perceptron.TeachAlpha(standartLetters[0], standartLetters[1], ref loading);
            }
            else if (gammaSystem.Checked)
            {
                perceptron.Responder.LoadWeigth(initialWeights);
                perceptron.TeachGamma(standartLetters[0], standartLetters[1], ref loading);
            }
            
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
                        inLetter[j] = 0;

                }
                if (perceptron.Recognize(inLetter) == 1)
                    MessageBox.Show("Sign№1");
                else
                    MessageBox.Show("Sign№2");
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
