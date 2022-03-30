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
    /// Логика взаимодействия для Add_post.xaml
    /// </summary>
    public partial class Add_post : Window
    {
        public Add_post()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Set_tovpost();
            Grid.ItemsSource = Util.postzakazis;
            Proverka();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_tovpost();
            Util.Clear_postav_add();
            Util.Clear_data_postav();
            Util.Clear_postav();
            Util.Set_postav();
        }

        private void Set_Data()
        {
            SqlCommand s1 = new SqlCommand("Select P.Data_post, W.Privet, Pk.Naim from Postavka P, Worker W, Postavshiki Pk where P.Sotr = W.Kod and P.Postav = Pk.Kod and P.Kod = " + Util.kod_post, Util.Get_k());
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                DateTime dt;
                string s, pn;
                dt = dr.GetDateTime(0);
                s = dr.GetString(1);
                pn = dr.GetString(2);
                Data_p.SelectedDate = dt;
                Sotr.Text = s;
                Util.Naim_set_p = pn;
            }
            dr.Close();
        }
        private void Proverka()
        {
            if (Util.kod_post == -1 || Util.kod_post == 0)
            {
                Data_p.SelectedDate = DateTime.Now;
            }
            else
            {
                Grid.IsReadOnly = true;
                Data_p.IsEnabled = false;
                plus_p.Visibility = Visibility.Collapsed;
                plus_s.Visibility = Visibility.Collapsed;
                Add_del.Visibility = Visibility.Collapsed;
                save_otm.Visibility = Visibility.Collapsed;
                Set_Data();
            }
        }

        private void plus_p_Click(object sender, RoutedEventArgs e)
        {
            Add_postavshik w = new Add_postavshik();
            w.ShowDialog();
        }

        private void plus_s_Click(object sender, RoutedEventArgs e)
        {
            Add_sotr w = new Add_sotr();
            w.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_tov_post w = new Add_tov_post();
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedIndex < 0)
            {

            }
            else
            {
                try
                {
                    Util.postzakazis.RemoveAt(Grid.SelectedIndex);
                    Util.Sum_ps = Util.Sum_post();
                }
                catch
                {
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Валидация
            if (Util.Kod_set_p == -1 || Util.Kod_set_s == -1 || Data_p.SelectedDate is null || Util.postzakazis.Count()<0)
            {
                MessageBox.Show("Введены не все данные!");
            }
            else
            {
                Util.Dt = Data_p.DisplayDate;
                bool a = Util.Add_full_post();
                if (a)
                {
                    MessageBox.Show("Данные добавлены!");
                }
                else
                {
                    MessageBox.Show("При добавлении данных произошла ошибка!");
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
