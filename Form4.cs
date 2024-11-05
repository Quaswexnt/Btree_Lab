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
    public partial class Form4 : Form
    {
        private BTree tree;

        public Form4(BTree sharedTree)
        {
            
            InitializeComponent();
            this.tree = sharedTree;
        }
        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int keyValue))
            {
                var existsCheck = tree.SearchKey(keyValue);
                if (existsCheck!=null)
                {
                    MessageBox.Show($"Значення для ключа {keyValue} : {existsCheck.Value.Item1.Keys[existsCheck.Value.Item2].Item2}");
                }
                else
                {
                    MessageBox.Show("Такого значення не існує в системі");
                }
            }
            else
            {
                MessageBox.Show("Невірне значення ключа!");
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
