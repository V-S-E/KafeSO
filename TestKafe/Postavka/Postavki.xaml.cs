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

namespace TestKafe.Postavka
{
    /// <summary>
    /// Логика взаимодействия для Postavki.xaml
    /// </summary>
    public partial class Postavki : Page
    {
        public Postavki()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.postavkis.Count() > 0)
            {
                Util.Clear_postav();
            }
            Util.Set_postav();
            LI.ItemsSource = Util.postavkis;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Add_post w = new Add_post();
            Util.kod_post = Convert.ToInt32(l.Tag);
            w.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_post w = new Add_post();
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
