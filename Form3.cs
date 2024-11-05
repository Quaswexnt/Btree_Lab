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
    public partial class Form3 : Form
    {
        private BTree tree;
        public Form3(BTree sharedTree)
        {
            InitializeComponent();
            this.tree = sharedTree;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int keyValue))
            {
                var existsCheck = tree.SearchKey(keyValue);
                if (existsCheck != null)
                {
                    var (node, index) = existsCheck.Value;
                    var value = node.Keys[index].Item2;

                    // Удаляем запись из дерева
                    tree.Delete(tree.root, (keyValue, value));

                    // Читаем все строки из файла
                    var lines = File.ReadAllLines(@"C:\Users\nnhf2\Desktop\PA_LABS\PA_lab_3\database.txt").ToList();

                    // Удаляем строку с данным ключом
                    lines.RemoveAll(line => line.StartsWith($"{keyValue} "));

                    // Перезаписываем файл с обновленными данными
                    File.WriteAllLines(@"C:\Users\nnhf2\Desktop\PA_LABS\PA_lab_3\database.txt", lines);

                    MessageBox.Show($"Значення для ключа {keyValue} : {value} Було видалено");
                }
                else
                {
                    MessageBox.Show("Значення під таким ключем не існує");
                }
            }
            else
            {
                MessageBox.Show("Невірне значення ключа!");
            }
        }
    }
}
