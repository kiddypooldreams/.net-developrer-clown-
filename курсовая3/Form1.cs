using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace курсовая3
{
    public partial class Form1 : Form
    {
        // Объеты для подключения
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable dataTable; // Автомобили
        public authorization ParentForm { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Установка соединения с базой данных
            connection = new SqlConnection(@"Data Source=LAPTOP-28N6K1IP\SQLEXPRESS;Initial Catalog=Kursovaya;Integrated Security=True");
            connection.Open();
            LoadCarData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрытие соединения при закрытии формы
            connection.Close();
        }

        public virtual void LoadCarData()
        {
            // Загрузка данных автомобилей из базы данных
            string query = "SELECT CarID as 'Код_машины', Brand as 'Бренд', Year as 'Год', Price as 'Цена_₽', TechnicalSpecifications as 'Марка', Features as 'Особенности', Condition as 'Состояние' FROM Cars";
            adapter = new SqlDataAdapter(query, connection);
            dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Загрузка уникальных значений бренда в ComboBox
            cmbBrand.DataSource = dataTable.AsEnumerable().Select(row => row.Field<string>("Бренд")).Distinct().ToList();
            cmbBrand.SelectedIndex = -1;

            // Загрузка уникальных значений года в ComboBox
            cmbYear.DataSource = dataTable.AsEnumerable().Select(row => row.Field<int>("Год")).Distinct().ToList();
            cmbYear.SelectedIndex = -1;

            dataGridViewCars.DataSource = dataTable;
        }

        public List<Car> GetSelectedCars()
        {
            // Получение списка выбранных автомобилей из DataGridView
            List<Car> selectedCars = new List<Car>();
            foreach (DataGridViewRow selectedRow in dataGridViewCars.SelectedRows)
            {
                int carID = Convert.ToInt32(selectedRow.Cells["Код_машины"].Value);
                string brand = selectedRow.Cells["Бренд"].Value.ToString();
                int year = Convert.ToInt32(selectedRow.Cells["Год"].Value);
                string technicalSpecs = selectedRow.Cells["Марка"].Value.ToString();
                double price = Convert.ToDouble(selectedRow.Cells["Цена_₽"].Value);
                string features = selectedRow.Cells["Особенности"].Value.ToString();
                string condition = selectedRow.Cells["Состояние"].Value.ToString();

                selectedCars.Add(new Car(carID, brand, year, technicalSpecs, price, condition, features));
            }

            return selectedCars;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Сброс фильтров и загрузка всех данных
            cmbBrand.SelectedIndex = -1;
            LoadCarData();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Фильтрация данных на основе выбранных параметров
            string selectedBrand = cmbBrand.SelectedItem?.ToString();
            int selectedYear = cmbYear.SelectedItem != null ? Convert.ToInt32(cmbYear.SelectedItem) : 0;
            double minPrice = string.IsNullOrEmpty(txtMinPrice.Text) ? 0 : Convert.ToDouble(txtMinPrice.Text);
            double maxPrice = string.IsNullOrEmpty(txtMaxPrice.Text) ? double.MaxValue : Convert.ToDouble(txtMaxPrice.Text);

            // Сброс выбранных параметров и текстовых полей
            cmbBrand.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            txtMinPrice.Text = "";
            txtMaxPrice.Text = "";

            // Создание SQL-запроса с учетом выбранных параметров
            string query = "SELECT CarID as 'Код_машины', Brand as 'Бренд', Year as 'Год', Price as 'Цена_₽', TechnicalSpecifications as 'Марка', Features as 'Особенности', Condition as 'Состояние' FROM Cars WHERE 1=1";

            if (!string.IsNullOrEmpty(selectedBrand))
                query += " AND Brand = @Brand";

            if (selectedYear > 0)
                query += " AND Year = @Year";

            query += " AND Price >= @MinPrice AND Price <= @MaxPrice";

            SqlCommand command = new SqlCommand(query, connection);

            if (!string.IsNullOrEmpty(selectedBrand))
                command.Parameters.AddWithValue("@Brand", selectedBrand);

            if (selectedYear > 0)
                command.Parameters.AddWithValue("@Year", selectedYear);

            command.Parameters.AddWithValue("@MinPrice", minPrice);
            command.Parameters.AddWithValue("@MaxPrice", maxPrice);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridViewCars.DataSource = dataTable;
            dataGridViewCars.AutoResizeColumns();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Обработка нажатия кнопки отправки заявки
            List<Car> selectedCars = GetSelectedCars();
            string contactName = txtContactName.Text;
            string contactEmail = txtContactEmail.Text;
            string selectedRequirements = RequirementsRichBox.Text;

            // Проверка введенных данных
            if (string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(contactEmail))
            {
                MessageBox.Show("Введите контактные данные");
            }
            else if (Regex.IsMatch(contactName, @"\d"))
            {
                MessageBox.Show("Имя контакта не может содержать числа");
            }
            else if (!Regex.IsMatch(contactEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                MessageBox.Show("Введите корректный адрес электронной почты");
            }
            else if (selectedCars.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль.");
            }
            else
            {
                // Создание формы подтверждения и передача данных
                ConfirmationForm confirmationForm = new ConfirmationForm();
                confirmationForm.SetData(contactName, contactEmail, selectedCars, selectedRequirements);
                DialogResult result = confirmationForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Отправка заявки в базу данных
                    foreach (Car selectedCar in selectedCars)
                    {
                        string insertQuery = "INSERT INTO Customers (ContactName, ContactEmail, CarID, Requirements) VALUES (@ContactName, @ContactEmail, @CarID, @Requirements)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@ContactName", contactName);
                        insertCommand.Parameters.AddWithValue("@ContactEmail", contactEmail);
                        insertCommand.Parameters.AddWithValue("@CarID", selectedCar.CarID);
                        insertCommand.Parameters.AddWithValue("@Requirements", selectedRequirements);

                        insertCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Заявка успешно отправлена.");
                }
                else
                {
                    MessageBox.Show("Отправка заявки отменена.");
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Показать родительскую форму при нажатии на кнопку "Назад"
            ParentForm.Show();
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Показать родительскую форму при закрытии текущей формы
            ParentForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Загрузка данных автомобилей
            LoadCarData();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Открыть форму справки
            Справка form = new Справка();
            form.Show();
        }
    }

    public class Car
    {
        // Класс, представляющий автомобиль
        public string Brand { get; set; }
        public int Year { get; set; }
        public string TechnicalSpecifications { get; set; }
        public double Price { get; set; }
        public int CarID { get; set; }
        public string Features { get; set; }
        public string Condition { get; set; }

        public Car(int carID, string brand, int year, string technicalSpecifications, double price, string features, string condition)
        {
            CarID = carID;
            Brand = brand;
            Year = year;
            TechnicalSpecifications = technicalSpecifications;
            Price = price;
            Features = features;
            Condition = condition;
        }
    }
}
