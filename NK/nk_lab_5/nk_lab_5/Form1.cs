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
        int maxLength;
        List<List<double>> weightsInit = new List<List<double>>();
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
        private void teach_Click(object sender, EventArgs e)
        {
            if (firstName.Text != "" && middleName.Text != "" && surName.Text != "" && alphaValue.Text != "" && rValue.Text != "" && kValue.Text != "")
            {
                string firstNameVal = firstName.Text;
                string middleNameVal = middleName.Text;
                string surNameVal = surName.Text;
                double alphaVal = Convert.ToDouble(alphaValue.Text);
                int radiusVal = Convert.ToInt32(rValue.Text);
                double coeff = Convert.ToDouble(kValue.Text);
                List<string> rows = new List<string>();
                List<List<int>> vectors = new List<List<int>>();
                rows.Add(firstNameVal);
                rows.Add(middleNameVal);
                rows.Add(surNameVal);
                maxLength = 0;
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].Length > maxLength)
                    {
                        maxLength = rows[i].Length;
                    }
                }
                Constants.sensorCount = maxLength;
                for (int j = 0; j < rows.Count; j++)
                {
                    List<int> vector = new List<int>();
                    List<int> vector1 = new List<int>();
                    List<int> vector2 = new List<int>();
                    for (int i = 0; i < rows[j].Length; i++)
                    {
                        if (rows[j][i] == 'a' || rows[j][i] == 'e' || rows[j][i] == 'y' || rows[j][i] == 'u' || rows[j][i] == 'i' || rows[j][i] == 'o' ||
                            rows[j][i] == 'у' || rows[j][i] == 'е' || rows[j][i] == 'ы' || rows[j][i] == 'а' || rows[j][i] == 'о' || rows[j][i] == 'э' || rows[j][i] == 'я' || rows[j][i] == 'и' || rows[j][i] == 'ю' || rows[j][i] == 'ё')
                        {
                            vector.Add(1);
                            vector1.Add(1);
                            vector2.Add(1);
                        }
                        else
                        {
                            vector.Add(0);
                            vector1.Add(0);
                            vector2.Add(0);
                        }
                    }
                    for (int i = vector.Count; i < maxLength; i++)
                    {
                        vector.Add(0);
                        vector1.Add(0);
                        vector2.Add(0);
                    }
                    vectors.Add(vector);
                    vectors.Add(vector1);
                    vectors.Add(vector2);
                }

                vectors = Inverse(1, 1, vectors);
                vectors = Inverse(2, 2, vectors);
                vectors = Inverse(4, 1, vectors);
                vectors = Inverse(5, 2, vectors);
                vectors = Inverse(7, 1, vectors);
                vectors = Inverse(8, 2, vectors);
                Log log = new Log();                
                log.WriteToLogString(" ========== VECTORS ========== ");
                log.WriteToLogHead(vectors[0], "x", 5);
                for (int i = 0; i < vectors.Count; i++)
                {
                    log.WriteToLog(vectors[i], "v", i);
                }

                log.WriteToLogString("α = " + alphaValue.Text + "; R = " + rValue.Text + "; k = " + kValue.Text);
                kohNet = new KohonenNet(vectors[0].Count, alphaVal, radiusVal, coeff);
                for (int j = 0; j < vectors.Count; j++)
                {
                    List<double> curW = new List<double>();
                    for (int i = 0; i < maxLength; i++)
                    {
                        curW.Add((double)rand.Next(1, 10) / 10);
                    }
                    weightsInit.Add(curW);
                }
                kohNet.InitAxons(weightsInit);
                if (kohNet.Teach(vectors))
                {
                    MessageBox.Show("Create successful by " + kohNet.Iteration.ToString() + " iterations. View 'log.txt' for more information");
                }
            }
            else
            {
                MessageBox.Show("Fill all fields!");
            }
            
        }

        /*private void recognize_Click(object sender, EventArgs e)
        {
            List<int> recVector = new List<int>();
            string rec = textBox1.Text;
            int length;
            if (rec.Length < maxLength)
            {
                length = rec.Length;
            }
            else
            {
                length = maxLength;
            }
            for (int i = 0; i < length; i++)
            {
                if (rec[i] == 'a' || rec[i] == 'e' || rec[i] == 'y' || rec[i] == 'u' || rec[i] == 'i' || rec[i] == 'o' ||
                    rec[i] == 'у' || rec[i] == 'е' || rec[i] == 'ы' || rec[i] == 'а' || rec[i] == 'о' || rec[i] == 'э' || rec[i] == 'я' || rec[i] == 'и' || rec[i] == 'ю' || rec[i] == 'ё')
                {
                    recVector.Add(1);
                }
                else
                {
                    recVector.Add(0);

                }
            }
            for (int i = recVector.Count; i < maxLength; i++)
            {
                recVector.Add(0);
            }
            int val = kohNet.Recognize(recVector);
            MessageBox.Show("Closer to " + val.ToString() + " word");
        }*/

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kononov Alexandr. CIT-20b (2015)");
        }
    }
}