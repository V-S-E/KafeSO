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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestKafe.Sklad
{
    /// <summary>
    /// Логика взаимодействия для Sklad.xaml
    /// </summary>
    public partial class Sklad : Page
    {
        public Sklad()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.skladis.Count() == 0)
            {
                Util.Clear_sklad();
            }
            Util.Set_sklad();
            LI.ItemsSource = Util.skladis;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List_spis w = new List_spis();
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_sklad();
            Util.Set_sklad();
            Util.poisk = "";
        }
    }
}
