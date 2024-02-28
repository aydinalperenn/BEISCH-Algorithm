using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CENG307__201180014__HW1
{
    public partial class FrmMain : Form
    {
        int keyNum;
        int tableNum;
        int[] keys;
        int mod;

        bool isMadeTable = false;

        // birinci int kısmı index
        // ikinci list kısmına sırasıyla Record, Link ve Probe gelecek

        Dictionary<int, List<int>> beischTable;

        public FrmMain()
        {
            InitializeComponent();
        }


        private void BeischAlgorithm()
        {
         
            if(isMadeTable == false)
            {
                BeischAlgorithm();
                return;
            }
            else
            {
                foreach (int key in keys)
                {
                    int probe = 0;
                    int index = key % mod;
                    bool lookForLast = true;


                    while (true)
                    {
                        probe++;

                        if (beischTable[index][0] == -1)
                        {
                            beischTable[index][0] = key;
                            beischTable[index][2] = probe;
                            break;
                        }
                        else
                        {

                            int linkOfBeforeNode = beischTable[index][1];

                            if (lookForLast)
                            {
                                // son değere bakacak
                                for (int indexLast = beischTable.Keys.Count - 1; indexLast >= 0; indexLast--)
                                {
                                    probe++;

                                    if (beischTable[indexLast][0] == -1)
                                    {
                                        beischTable[indexLast][0] = key;
                                        beischTable[indexLast][1] = linkOfBeforeNode;
                                        beischTable[indexLast][2] = probe;
                                        beischTable[index][1] = indexLast;
                                        lookForLast = !lookForLast;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // ilk değere bakacak
                                for (int indexFirst = 0; indexFirst < beischTable.Keys.Count; indexFirst++)
                                {
                                    probe++;

                                    if (beischTable[indexFirst][0] == -1)
                                    {
                                        beischTable[indexFirst][0] = key;
                                        beischTable[indexFirst][1] = linkOfBeforeNode;
                                        beischTable[indexFirst][2] = probe;
                                        beischTable[index][1] = indexFirst;
                                        lookForLast = !lookForLast;
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }

                }

                WriteToBeischDatagrid();
            }

            

        }
        

        private void WriteToBeischDatagrid()
        {
            dgridBeisch.Columns.Clear();
            dgridBeisch.Rows.Clear();

            // DataGridView'e sütunları ekle
            dgridBeisch.Columns.Add("IndexColumn", "Index");
            dgridBeisch.Columns.Add("RecordColumn", "Record");
            dgridBeisch.Columns.Add("LinkColumn", "Link");
            dgridBeisch.Columns.Add("ProbeColumn", "Probe");


            foreach (var kvp in beischTable)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewCell keyCell = new DataGridViewTextBoxCell();
                keyCell.Value = kvp.Key;
                row.Cells.Add(keyCell);

                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    DataGridViewCell valueCell = new DataGridViewTextBoxCell();

                    if (kvp.Value[i] != -1)
                    {
                        valueCell.Value = kvp.Value[i];

                    }
                    else
                    {
                        valueCell.Value = null;

                    }

                    row.Cells.Add(valueCell);
                }

                dgridBeisch.Rows.Add(row);
            }
        }



        private void MakeTables()
        {
            beischTable = new Dictionary<int, List<int>>();
            for(int i = 0; i < tableNum; i++)
            { 
                beischTable.Add(i, new List<int> { -1, -1, -1 });
            }

            isMadeTable = true;

        }

        private void FindModValue()
        {
            listBox1.Items.Clear();

            int num = keyNum+1;
            while (true)
            {
                int prime = 0;

                for (int i = 2; i < num; i++)
                {
                    if (num % i == 0)
                    {
                        prime++;
                        break;

                    }
                
                }

                if (prime == 0)
                {
                    mod = num;
                    break;
                }

                num++;
            }
            listBox1.Items.Add("Mod: " + mod);

        }
        private void InstantiateKeys(int num)
        {

            keys = new int[num];
            Random random = new Random();

            for (int i = 0; i < num; i++)
            {
                keys[i] = random.Next(1, 1001);
            }

        }


        private void btnCompare_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;


            if (mTxtKeyNum.Text.Length == 0)
            {
                MessageBox.Show("Anahtar sayısı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (mTxtTableRow.Text.Length == 0)
                {
                    MessageBox.Show("Satır sayısı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (Convert.ToInt32(mTxtKeyNum.Text) > 1000)
                    {
                        MessageBox.Show("En fazla 1000 rastgele anahtar olmalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (Convert.ToInt32(mTxtKeyNum.Text) >= Convert.ToInt32(mTxtTableRow.Text))
                        {
                            MessageBox.Show("Satır sayısı anahtar sayısından küçük veya aynı olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            keyNum = Convert.ToInt32(mTxtKeyNum.Text);
                            tableNum = Convert.ToInt32(mTxtTableRow.Text);
                            InstantiateKeys(keyNum);
                            MakeTables();
                            FindModValue();
                            BeischAlgorithm();

                        }


                    }

                }

            }



            Cursor = Cursors.Default;
        }

    }
}
