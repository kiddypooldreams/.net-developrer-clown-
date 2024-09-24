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
using System.IO;

namespace курсовая3
{
    public partial class adminMenu : Form
    {
        private Form1 form1;
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        public authorization ParentForm { get; set; } 
        public adminMenu()
        {
            InitializeComponent();
           
            form1 = new Form1();
        }

        private void LoadCustomersData()
        {
            string query = "SELECT CustomerID as 'ID_клиента', ContactName as 'Имя_контакта', ContactEmail as 'Email', Requirements as 'Требования', CarID as 'ID_машины', RequestStatus FROM Customers";

            bool hasRequestStatusColumn = dataGridViewCustomers.Columns.Contains("RequestStatus");

            if (!hasRequestStatusColumn)
            {
                DataGridViewCheckBoxColumn statusColumn = new DataGridViewCheckBoxColumn();
                statusColumn.HeaderText = "RequestStatus";
                statusColumn.Name = "RequestStatus";
                statusColumn.DataPropertyName = "RequestStatus"; ;

                dataGridViewCustomers.Columns.Add(statusColumn);
            }

            adapter = new SqlDataAdapter(query, connection);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridViewCustomers.DataSource = dataTable;

            dataGridViewCustomers.CellValueChanged += (sender, e) =>
            {
                if (e.ColumnIndex == dataGridViewCustomers.Columns["RequestStatus"].Index && e.RowIndex >= 0)
                {
                    // Получаем значение чекбокса
                    string cellValue = dataGridViewCustomers.Rows[e.RowIndex].Cells["RequestStatus"].Value.ToString();

                    // Преобразуем значение строки в тип bool
                    bool newValue;
                    if (!bool.TryParse(cellValue, out newValue))
                    {
                        // Если преобразование не удалось, выводим сообщение об ошибке и возвращаемся
                        MessageBox.Show("Неверное значение для Статус запроса");
                        return;
                    }



                    // Получаем значение первичного ключа (если есть) для данной строки
                    int primaryKeyValue = Convert.ToInt32(dataGridViewCustomers.Rows[e.RowIndex].Cells["ID_клиента"].Value);

                    // Обновляем значение в базе данных
                    string updateQuery = "UPDATE Customers SET RequestStatus = @RequestStatus WHERE CustomerID = @CustomerID"; 
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@RequestStatus", cellValue);
                    updateCommand.Parameters.AddWithValue("@CustomerID", primaryKeyValue);
                    updateCommand.ExecuteNonQuery();
                }

            };
        }

        private void adminMenu_Load(object sender, EventArgs e)
        {
            


            connection = new SqlConnection(@"Data Source=LAPTOP-28N6K1IP\SQLEXPRESS;Initial Catalog=Kursovaya;Integrated Security=True");
            connection.Open();
            LoadCarData();
            LoadCustomersData();

        }
        private void adminMenuClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }

        private void LoadCarData()
        {
            string query = "SELECT CarID as 'Код_машины', Brand as 'Бренд', Year as 'Год', Price as 'Цена', TechnicalSpecifications as 'Марка', Features as 'Особенности', Condition as 'Состояние' FROM Cars";

            adapter = new SqlDataAdapter(query, connection);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridViewCars.DataSource = dataTable;

            dataGridViewCars.Columns["Код_машины"].HeaderText = "Код_машины";
            dataGridViewCars.Columns["Бренд"].HeaderText = "Бренд";
            dataGridViewCars.Columns["Год"].HeaderText = "Год";
            dataGridViewCars.Columns["Цена"].HeaderText = "Цена";
            dataGridViewCars.Columns["Марка"].HeaderText = "Марка";
            dataGridViewCars.Columns["Особенности"].HeaderText = "Особенности";
            dataGridViewCars.Columns["Состояние"].HeaderText = "Состояние";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            AddNewCar addNewCarForm = new AddNewCar(this);
            addNewCarForm.Show();
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0) // Проверяем, есть ли выбранная строка
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите удалить выбранные строки?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow selectedRow in dataGridViewCustomers.SelectedRows)
                    {
                        int customerID = Convert.ToInt32(selectedRow.Cells["ID_клиента"].Value);

                        string query = "DELETE FROM Customers WHERE CustomerID = @customerID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@customerID", customerID);
                        command.ExecuteNonQuery();
                    }

                    // Обновляем данные в таблице
                    LoadCustomersData();
                }
            }
            else
            {
                MessageBox.Show("Веберите строку!");
            }
        }


        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.SelectedRows.Count > 0) {
                DialogResult result = MessageBox.Show("Вы действительно хотите удалить выбранные строки?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow selectedRow in dataGridViewCars.SelectedRows)
                    {
                        int carID = Convert.ToInt32(selectedRow.Cells["Код_машины"].Value);

                        string query = "DELETE FROM Cars WHERE CarID = @carID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@carID", carID);
                        command.ExecuteNonQuery();
                    }

                    // Обновляем данные в таблице
                    LoadCarData();
                }
            }
            else {
                MessageBox.Show("Веберите строку!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadCarData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadCustomersData();
        }

        private void RequestCheck_Click(object sender, EventArgs e)
        {
            string currentDateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string fileName = $"Заявки_на_приобретение_машин_{currentDateTime}.txt";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            saveFileDialog.Title = "Сохранить данные в файл";
            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Открываем файл для записи
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    // Перебираем строки DataGridView
                    foreach (DataGridViewRow row in dataGridViewCustomers.Rows)
                    {
                        DataGridViewCell cell = row.Cells["RequestStatus"];
                        if (cell != null && cell.Value != null)
                        {
                            // Получаем значение столбца RequestStatus
                            string requestStatusString = cell.Value.ToString();
                            bool requestStatus;
                            if (!bool.TryParse(requestStatusString, out requestStatus))
                            {
                                // Если преобразование не удалось или значение ячейки некорректно, пропускаем текущую строку
                                continue;
                            }

                            // Если RequestStatus равно true, записываем значения в файл
                            if (requestStatus)
                            {
                                // Получаем значения других столбцов
                                int customerId = (int)row.Cells["ID_клиента"].Value;
                                string contactName = (string)row.Cells["Имя_контакта"].Value;
                                string contactEmail = (string)row.Cells["Email"].Value;
                                string requirements = (string)row.Cells["Требования"].Value;
                                int carId = (int)row.Cells["ID_машины"].Value;

                                // Записываем значения в файл
                                writer.WriteLine($"ID_клиента: {customerId}");
                                writer.WriteLine($"Имя_контакта: {contactName}");
                                writer.WriteLine($"Email: {contactEmail}");
                                writer.WriteLine($"Требования: {requirements}");
                                writer.WriteLine($"ID_машины: {carId}");
                                writer.WriteLine();
                            }
                        }
                    }
                }

                MessageBox.Show("Данные успешно сохранены в файл.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewCustomers.DataSource as DataTable).DefaultView.RowFilter = $"Имя_контакта LIKE '%{textBox1.Text}%'";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
             (dataGridViewCustomers.DataSource as DataTable).DefaultView.RowFilter = $"Email LIKE '%{textBox2.Text}%'";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        { 
           if (int.TryParse(textBox3.Text, out int id))
           {
                (dataGridViewCustomers.DataSource as DataTable).DefaultView.RowFilter = $"ID_машины = {id}"; 
           }

            else
            {
                MessageBox.Show("Некорректный ввод");
                (dataGridViewCustomers.DataSource as DataTable).DefaultView.RowFilter = "";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox4.Text, out int idcar))
            {
                (dataGridViewCars.DataSource as DataTable).DefaultView.RowFilter = $"Код_машины = {idcar}";
            }
            else
            {
                MessageBox.Show("Некорректный ввод");
                (dataGridViewCars.DataSource as DataTable).DefaultView.RowFilter = "";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox5.Text, out int year))
            {
                (dataGridViewCars.DataSource as DataTable).DefaultView.RowFilter = $"Год = {year}";
            }
            else
            {
                MessageBox.Show("Некорректный ввод");
                (dataGridViewCars.DataSource as DataTable).DefaultView.RowFilter = "";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            (dataGridViewCars.DataSource as DataTable).DefaultView.RowFilter = $"Бренд LIKE '%{textBox6.Text}%'";
        }

        private void adminMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParentForm.Show();
        }

        private void авторизацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParentForm.Show(); // Показываем родительскую форму
            Close();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Справка form = new Справка();
            form.Show();
            
        }
    }
}
