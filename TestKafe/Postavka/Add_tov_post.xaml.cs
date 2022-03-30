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
using System.Data.SqlClient;

namespace TestKafe.Postavka
{
    /// <summary>
    /// Логика взаимодействия для Add_tov_post.xaml
    /// </summary>
    public partial class Add_tov_post : Window
    {
        public Add_tov_post()
        {
            InitializeComponent();
            Util.Set_tovar_post();
            LI.ItemsSource = Util.tovarisadd;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_tovar_post();
            Util.Set_tovar_post();
            Util.poisk = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Set_tovar_post();
            LI.ItemsSource = Util.tovarisadd;
            CB.ItemsSource = Util.raz_tov_add;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_tovar_post();
            Util.ed_t = "";
            Util.raz_tov_add.Clear();
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.Clear_raz_tov_add();
            Util.Set_raz_tov_add(l.Tag.ToString());
            Tov_n.Text = l.Content.ToString();
            Tov_n.Tag = l.Tag;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CB.SelectedIndex < 0)
            {
                MessageBox.Show("Товар выбран некорректно!");
            }
            else
            {
                int kt, kr;
                string nt, nr, ne;

                kt = Convert.ToInt32(Tov_n.Tag.ToString());
                nt = Tov_n.Text;
                kr = Util.raz_tov_add[CB.SelectedIndex].kod;
                nr = Util.raz_tov_add[CB.SelectedIndex].nazv;
                Util.Set_ed_t(kt);
                ne = Util.ed_t;
                Util.postzakazis.Add(new Post_zakaz(kt, nt, kr, nr, ne));
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
