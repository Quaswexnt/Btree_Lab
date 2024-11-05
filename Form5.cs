using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Btree_LaB
{
    public partial class Form5 : Form
    {
        private BTree tree;
        public Form5(BTree sharedTree)
        {
            InitializeComponent();
            this.tree = sharedTree;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int keyValue))
            {
                string value = textBox2.Text.Trim();

                bool checkExists = tree.ContainsKey(keyValue);

                if (checkExists)
                {
                    
                    tree.Update(keyValue, value);

                   
                    var lines = File.ReadAllLines(@"C:\Users\nnhf2\Desktop\PA_LABS\PA_lab_3\database.txt").ToList();

                    
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].StartsWith($"{keyValue} "))
                        {
                            lines[i] = $"{keyValue} {value}";
                            break;
                        }
                    }

                    
                    File.WriteAllLines(@"C:\Users\nnhf2\Desktop\PA_LABS\PA_lab_3\database.txt", lines);

                    MessageBox.Show("Дані успішно редаговано");
                }
                else
                {
                    MessageBox.Show("Такого ключа немає у системі");
                }
            }
            else
            {
                MessageBox.Show("Невірне значення ключа!");
            }
        }
    }
}
