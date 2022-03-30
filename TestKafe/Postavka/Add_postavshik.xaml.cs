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
    /// Логика взаимодействия для Add_postavshik.xaml
    /// </summary>
    public partial class Add_postavshik : Window
    {
        public Add_postavshik()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.add_post.Count != 0)
            {
                Util.Clear_postav_add();
            }
            Util.Set_postav_add();
            LI.ItemsSource = Util.add_post;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_postav_add();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_postav_add();
            Util.Set_postav_add();
            Util.poisk = "";
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.Naim_set_p = l.Content.ToString();
            Util.Kod_set_p = Convert.ToInt32(l.Tag);
            this.Close();
        }
    }
}
