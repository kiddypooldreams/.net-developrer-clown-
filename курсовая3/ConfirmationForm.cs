using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсовая3
{
    public partial class ConfirmationForm : Form
    {
        private List<Car> selectedCars;

        public ConfirmationForm()
       
        {
            InitializeComponent();
        }

        public void SetData(string contactName, string contactEmail, List<Car> selectedCars, string selectedRequirements)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ФИО: " + contactName);
            sb.AppendLine("Email: " + contactEmail);
            sb.AppendLine("Пожелания: " + selectedRequirements);
            sb.AppendLine("Номер машины: " + string.Join(", ", selectedCars.Select(car => car.CarID)));

            // Устанавливаем текст в RichTextBox
            richTextBox1.Text = sb.ToString();
        }

        private void ConfirmationForm_Load(object sender, EventArgs e)
        {

        }

        private void CustomButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
