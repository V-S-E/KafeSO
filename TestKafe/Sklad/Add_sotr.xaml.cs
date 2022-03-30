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

namespace TestKafe.Sklad
{
    /// <summary>
    /// Логика взаимодействия для Add_sotr.xaml
    /// </summary>
    public partial class Add_sotr : Window
    {
        public Add_sotr()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Set_sotr_add();
            LI.ItemsSource = Util.add_sotr;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_sotr_add();
            Util.poisk = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_sotr_add();
            Util.Set_sotr_add();
            Util.poisk = "";
        }
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.Sp_add.Sotr.fio = l.Content.ToString();
            Util.Sp_add.Sotr.kod = Convert.ToInt32(l.Tag);
            Util.Clear_sotr_add();
            this.Close();
        }
    }
}
