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
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace TestKafe.Ingredient
{
    /// <summary>
    /// Логика взаимодействия для List_ingr.xaml
    /// </summary>
    public partial class List_ingr : Page
    {
        public List_ingr()
        {
            InitializeComponent();
            if (Util.collect_ing.Count() > 0)
            {
                Util.Clear_list_ingr();
            }
            Util.Set_list_ingr();
            LI.ItemsSource = Util.collect_ing;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ingr_w w = new Ingr_w();
            w.ShowDialog();
        }
        private void Label_click(object sender, RoutedEventArgs e)
        {
            Label l = sender as Label;
            Ingr_w i = new Ingr_w();
            Util.Set_kod_i(l.Tag.ToString());
            Util.Set_razn();
            Util.Set_tovar();
            i.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_list_ingr();
            Util.Set_list_ingr();
            Util.poisk = "";
        }
    }
}
