using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestKafe
{
    /// <summary>
    /// Логика взаимодействия для Autoriz.xaml
    /// </summary>
    public partial class Autoriz : Window
    {
        public Autoriz()
        {
            InitializeComponent();
            Util.FileCheck();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (Log.Text == "" || Pass.Password == "")
            {
                MessageBox.Show("Не все поля заполнены.");
                flag = false;
            }
            else
            {
                if(Util.Access(Log.Text, Pass.Password))
                {
                    MessageBox.Show("Ошибка подключения. Проверьте правильность данных или настройки.");
                    flag = false;
                }
            }
            if (flag == true)
            {
                Main w = new Main();
                w.Show();
                this.Close();
            }
        }
    }
}
