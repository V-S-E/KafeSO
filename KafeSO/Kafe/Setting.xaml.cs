using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Load(object s, EventArgs e)
        {
            Util.Check.Include_File();
            bool b = Util.Check.Check_File();
            if(b==false)
            {
                this.StringDBK.Text = Util.Check.Return_Value();
            }
            else
            {
                MessageBox.Show("Ошибка содержимого файла. Проверьте настройки");
            }
        }

        bool prov;
        private void Click_Connect(Object sender, RoutedEventArgs e)
        {
            Util.Check.Set_ConStr(Convert.ToString(Convert.ToString(StringDBK.GetLineText(0))));

            SqlConnection conK = Util.ConnectBD.ConnectKDB();

            try
            {
                conK.Open();
                MessageBox.Show("База Кафе подключена.");
                prov = true;
                conK.Close();
                this.Close();
                Load l = new Load();
                l.Show();
            }
            catch (Exception ex)
            {
                Util.Check.Clear_File();
                MessageBox.Show("Произошли ошибки подключения. Проверьте правильность ввода строк. \n\n Подключение вызвало исключение: " + ex.Message);
                this.StringDBK.Text = Util.Check.Return_Value();
            }
        }

        private void Close(object sender, EventArgs e)
        {
            if (!prov)
            {
                Application.Current.Shutdown();
            }
            else
            {
                Load l = new Load();
                l.Show();
            }
        }

    }
}
