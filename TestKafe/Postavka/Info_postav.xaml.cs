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

namespace TestKafe.Postavka
{
    /// <summary>
    /// Логика взаимодействия для Info_postav.xaml
    /// </summary>
    public partial class Info_postav : Window
    {
        public Info_postav()
        {
            InitializeComponent(); DataContext = Util.p;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Set_post_info();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Util.p.Kod == 0)
            {
                bool prov = Util.Save_post_info();
                if (prov == false)
                {
                    MessageBox.Show("При сохранении данных произошла ошибка.");
                }
                else
                {
                    Util.Clear_postav_add();
                    Util.Set_postav_add();
                    this.Close();
                }
            }
            else
            {
                bool prov = Util.Update_post_info();
                if (prov == false)
                {
                    MessageBox.Show("При обновлении данных произошла ошибка.");
                }
                else
                {
                    Util.Clear_postav_add();
                    Util.Set_postav_add();
                    this.Close();
                }
            }
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Integer(object sender, TextCompositionEventArgs e)
        {
            string rule = "1234567890";
            e.Handled = (rule).IndexOf(e.Text) < 0;
        }

        private void Text(object sender, TextCompositionEventArgs e)
        {
            string rule = "1234567890,./?\\][{}*&^%$#@!)(№;%:=+-";
            e.Handled = (rule).IndexOf(e.Text) > 0;
        }
    }
}
