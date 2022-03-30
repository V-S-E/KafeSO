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
    /// Логика взаимодействия для List_spis.xaml
    /// </summary>
    public partial class List_spis : Window
    {
        public List_spis()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Clear_sp();
            Util.Set_sp();
            LI.ItemsSource = Util.sp;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_sp();
            Util.Clear_sklad();
            Util.Set_sklad();
        }

        private void LI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Util.index_sp = LI.SelectedIndex;
            Spisanie w = new Spisanie();
            w.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.index_sp = -1;
            Spisanie w = new Spisanie();
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_sp();
            Util.Set_sp();
            Util.poisk = "";
        }
    }
}
