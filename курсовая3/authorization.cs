using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace курсовая3
{

    public partial class authorization : Form
    {
        private SqlConnection connection;
       
        public authorization()
        {
            InitializeComponent();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ParentForm = this; // Устанавливаем ссылку на родительскую форму
            form.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            string query = "SELECT COUNT(*) FROM Users WHERE Login = @login AND Password = @password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);

            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Успешная авторизация");
                textBox2.Text = "";
                textBox1.Text = "";
                adminMenu form = new adminMenu();
                form.ParentForm = this; // Устанавливаем ссылку на родительскую форму
                form.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Неверныйй логин или пароль");
            }
        }

        private void authorization_Load(object sender, EventArgs e)
        {
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            connection = new SqlConnection(@"Data Source=LAPTOP-28N6K1IP\SQLEXPRESS;Initial Catalog=Kursovaya;Integrated Security=True");
            connection.Open(); // Открытие соединения с базой данных
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Справка form = new Справка();
            form.Show();
            
        }
    }
}
