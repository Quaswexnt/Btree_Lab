using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Btree_LaB
{
    public partial class Form2 : Form
    {
        private BTree tree;

        public Form2(BTree sharedTree)
        {
            InitializeComponent();
            this.tree = sharedTree;
            
           
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (int.TryParse(textBox1.Text, out int keyValue))
            {
                
                string value = textBox2.Text.Trim();

               
                bool searchResult = tree.ContainsKey(keyValue);
                if (searchResult)
                {
                    
                    MessageBox.Show("Цей ключ вже є в системі");
                }
                else
                {
                    
                    tree.Insert((keyValue, value));

                    
                    File.AppendAllText(@"C:\Users\nnhf2\Desktop\PA_LABS\PA_lab_3\database.txt", $"{keyValue} {value}\n");

                   
                    MessageBox.Show("Нова пара ключ-значення додана до системи");
                }
            }
            else
            {
                
                MessageBox.Show("Невірне значення ключа!");
            }
        }
    }
}
