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
    public partial class Справка : Form
    {
        public authorization ParentForm { get; set; }
        public Справка()
        {
            InitializeComponent();
        }

        private void Справка_Load(object sender, EventArgs e)
        {
            label1.Text = "Вас приветствует приложение автоматизированной системы центра по продаже автомобилей.\n\nПриложение имеет два режима:\nа) Отправка заявки\nб) Управление заявками\n\nа) Для поиска автомобиля выберите с помощью выпадающего списка нужный бренд или год, при необходимости введите цену. Затем нажмите кнопку 'Поиск'. Чтобы отправить заявку, выделите нужную строку в таблице и введите контактные данные.\n\nб) В этом режиме вам доступны следующие действия: добавление и удаление машин в таблице, удаление и учет заявок в таблице клиентов. Чтобы отправить заявку клиента на выполнение, нажмите на ячейку 'RequestStatus' в соответствующей заявке и затем нажмите кнопку 'Учет заявки'.";
            
        }




        private void label1_Click(object sender, EventArgs e)
        {
            


        }
    }
}
