namespace Btree_LaB
{
    public partial class DataBase : Form
    {

        BTree tree = new BTree(25);

        
        public DataBase()
        {

            InitializeComponent();
            LoadData();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void LoadData()
        {
            string filePath = "C:\\Users\\nnhf2\\Desktop\\PA_LABS\\PA_lab_3\\database.txt";

            try
            {
                if (File.Exists(filePath))
                {
                    foreach (var line in File.ReadLines(filePath))
                    {
                        var parts = line.Split(' ', 2);
                        if (parts.Length == 2 && int.TryParse(parts[0], out int key))
                        {
                            // Добавляем проверку на null перед вызовом Insert
                            if (tree != null)
                            {
                                tree.Insert((key, parts[1].Trim()));
                            }
                            else
                            {
                                MessageBox.Show("Дерево не инициализировано.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Файл базы данных не найден.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenNestedForm2();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenNestedForm3();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenNestedForm4();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenNestedForm5();
        }

        private void OpenNestedForm2()
        {
            Form2 form2 = new Form2(tree);
            this.Hide();
            form2.ShowDialog(); 
            this.Show(); 
        }

        private void OpenNestedForm4()
        {
            Form4 form4 = new Form4(tree);
            this.Hide(); 
            form4.ShowDialog(); 
            this.Show(); 
        }
        private void OpenNestedForm3()
        {
            Form3 form3 = new Form3(tree);
            this.Hide();
            form3.ShowDialog();
            this.Show();
        }
        private void OpenNestedForm5()
        {
            Form5 form5 = new Form5(tree);
            this.Hide();
            form5.ShowDialog();
            this.Show();
        }




    }
}
