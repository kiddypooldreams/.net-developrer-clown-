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
    public partial class AddNewCar : Form
    {

        private adminMenu form1;
        private SqlConnection connection;
        
        public AddNewCar(adminMenu mainForm)
        {
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            form1 = mainForm;
            InitializeComponent();
        }

        private void AddNewCar_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(@"Data Source=LAPTOP-28N6K1IP\SQLEXPRESS;Initial Catalog=Kursovaya;Integrated Security=True");
            connection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int year;
            double price;
            if (int.TryParse(textBox2.Text, out year) && double.TryParse(textBox3.Text, out price))
            {
                string brand = textBox1.Text;

                string techicalspecifications = textBox4.Text;
                string features = textBox5.Text;
                string Condition = textBox6.Text;

                string insertQuery = "INSERT INTO Cars (Brand, Year, Price, TechnicalSpecifications, Features, Condition) VALUES (@Brand, @Year, @Price, @TechnicalSpecifications, @Features, @Condition)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Brand", brand);
                insertCommand.Parameters.AddWithValue("@Year", year);
                insertCommand.Parameters.AddWithValue("@Price", price);
                insertCommand.Parameters.AddWithValue("@TechnicalSpecifications", techicalspecifications);
                insertCommand.Parameters.AddWithValue("@Features", features);
                insertCommand.Parameters.AddWithValue("@Condition", Condition);

                insertCommand.ExecuteNonQuery();



                MessageBox.Show("Машина успешно добавлена.");

            }
            else
            {
                MessageBox.Show("Неверное значение year или price   ");
            }
        }
    }
}
