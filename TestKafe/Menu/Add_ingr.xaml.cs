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

namespace TestKafe.Menu
{
    /// <summary>
    /// Логика взаимодействия для Add_ingr.xaml
    /// </summary>
    public partial class Add_ingr : Window
    {
        public Add_ingr()
        {
            InitializeComponent();
            Util.Set_list_ingr();
            LI.ItemsSource = Util.collect_ing;
            CB.ItemsSource = Util.raz_ingr;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.Set_raz_ingr(l.Tag.ToString());
            Ingr_n.Text = l.Content.ToString();
            Ingr_n.Tag = l.Tag;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CB.SelectedIndex < 0)
            {
                MessageBox.Show("Товар выбран некорректно!");
            }
            else
            {
                int ki;
                string ni;
                float cena;
                Raznovidn_tov r = new Raznovidn_tov();
                Edinitci ed = new Edinitci();

                ki = Convert.ToInt32(Ingr_n.Tag.ToString());
                ni = Ingr_n.Text;
                r = Util.raz_ingr[CB.SelectedIndex];
                ed = Util.Set_edin_ingr(ki);
                cena = (float)Util.Set_cena_ingr(ki);
                Util.sm.sostav.Add(new Sostav_menu_c { Kod_i = ki, Nazv_i = ni, Ed = ed, Razn = r, Cena = cena });
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_list_ingr();
            Util.Set_list_ingr();
            Util.poisk = "";
        }
    }
}
