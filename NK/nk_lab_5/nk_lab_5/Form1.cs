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
        public Form1()
        {
            InitializeComponent();
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
            File.WriteAllText("log.txt", "");
            string firstNameVal = firstName.Text;
            string middleNameVal = middleName.Text;
            string surNameVal = surName.Text;
            List<string> rows = new List<string>();
            List<List<int>> vectors = new List<List<int>>(); 
            rows.Add(firstNameVal);
            rows.Add(middleNameVal);
            rows.Add(surNameVal);
            int maxLength = 0;
            for (int i = 0; i < rows.Count; i++ )
            {
                if (rows[i].Length > maxLength)
                {
                    maxLength = rows[i].Length;
                }
            }
            Constants.sensorCount = maxLength;

            /*string maxStr = rows[num];
            maxStr = maxStr.ToLower();
            List<int> firstNameVector = new List<int>();
            List<int> middleNameVector = new List<int>();
            List<int> surNameVector = new List<int>();*/
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
                for (int i = vector.Count; i < maxLength; i++ )
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
