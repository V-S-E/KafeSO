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

namespace TestKafe.Ingredient
{
    /// <summary>
    /// Логика взаимодействия для Add_tov.xaml
    /// </summary>
    public partial class Add_tov : Window
    {
        public Add_tov()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Переменные для хранения данных товара
        /// </summary>

        string nazv_t;
        string nazv_i;
        int ed;
        int kol_ed;
        double kol_ob;
        int srok;
        bool check;
        public void Get_data()
        {
            SqlCommand s11 = new SqlCommand("Select top 1 T.Nazv, I.Nazv, T.ed, T.Kol_ed, T.Kol_ob, T.Srok from Tovari T, Ingredient I, ed where T.ed = ed.Kod and T.Ingr = I.Kod and T.Kod =" + Util.kod_t.ToString() + " and T.Ingr = " + Util.Get_kod_i(), Util.Get_k());
            SqlDataReader sdr = s11.ExecuteReader();
            while (sdr.Read())
            {
                nazv_t = sdr.GetString(0).Trim();
                nazv_i = sdr.GetString(1).Trim();
                ed = sdr.GetInt32(2);
                kol_ed = sdr.GetInt32(3);
                kol_ob = sdr.GetDouble(4);
                srok = sdr.GetInt32(5);
            }
            sdr.Close();
        }

        public void Checked()
        {
            SqlCommand s11 = new SqlCommand("Select top 1 I.Ves from Ingredient I where I.Kod = " + Util.Get_kod_i(), Util.Get_k());
            check = Convert.ToBoolean(s11.ExecuteScalar());
        }

        public void Set_data()
        {
            Nazvanie.Text = nazv_t;
            Nazvingr.Content = nazv_i;
            int index = Util.edin.FindIndex(item => item.kod_ed == ed);
            edi.SelectedIndex = index;
            Koled.Text = kol_ed.ToString();
            Vesed.Text = kol_ob.ToString();
            Srokg.Text = srok.ToString();
        }

        public void Check_view(bool c)
        {
            if (c)
            {
                edves.Visibility = Visibility.Collapsed;
                Koled.Visibility = Visibility.Collapsed;
                vesed.Visibility = Visibility.Collapsed;
                edi.Visibility = Visibility.Collapsed;
            }
            else
            {
                Koled.Visibility = Visibility.Visible;
                edves.Visibility = Visibility.Visible;
                vesed.Visibility = Visibility.Visible;
                edi.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Checked();
            if (Util.kod_t == -1)
            {
            }
            else
            {
                Get_data();
                Set_data();
                Koled.IsReadOnly = true;
                Vesed.IsReadOnly = true;
            }
            Check_view(check);
            edi.ItemsSource = Util.edin;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.kod_t = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (check)
            {
                if (Nazvanie.Text.Trim() == "" || Srokg.Text.Trim() == "")
                {
                    MessageBox.Show("Введите данные!");
                }
                else
                {
                    if (Util.kod_t == -1)
                    {
                        SqlCommand s1 = new SqlCommand("insert into Tovari (Nazv,Srok,Ingr,ed) values (N'" + Nazvanie.Text + "', " + Srokg.Text + ", " + Util.Get_kod_i() + "," + Util.kod_ed_ingr + ")", Util.Get_k());
                        s1.ExecuteNonQuery();
                        SqlCommand s2 = new SqlCommand("Select top 1 kod from Tovari order by Kod desc", Util.Get_k());
                        Util.kod_t = Convert.ToInt32(s2.ExecuteScalar().ToString());
                        Util.Clear_tovari();
                        Util.Set_tovar();
                    }
                    this.Close();
                }
            }
            else
            {
                if ((Nazvanie.Text.Trim() == "" || Nazvanie.Text is null) || (Srokg.Text.Trim() == "" || Srokg.Text is null) || edi.SelectedIndex == -1 || (Koled.Text.Trim() == "" || Koled.Text is null ))
                {
                    MessageBox.Show("Введите данные!");
                }
                else
                {
                    if (Util.kod_t == -1)
                    {
                        SqlCommand s1 = new SqlCommand("insert into Tovari (Nazv,Srok,Ingr,ed,Kol_ed,Kol_ob) values (N'" + Nazvanie.Text + "', " + Srokg.Text + ", " + Util.Get_kod_i() + "," + Util.edin[edi.SelectedIndex].kod_ed + ", "+ Koled.Text.Trim() +", @ves )", Util.Get_k());
                        SqlParameter p2 = new SqlParameter("@ves", Convert.ToSingle(Vesed.Text.Trim()));
                        s1.Parameters.Add(p2);
                        s1.ExecuteNonQuery();
                        SqlCommand s2 = new SqlCommand("Select top 1 kod from Tovari order by Kod desc", Util.Get_k());
                        Util.kod_t = Convert.ToInt32(s2.ExecuteScalar().ToString());
                        Util.Clear_tovari();
                        Util.Set_tovar();
                        this.Close();
                    }
                }
            }
        }

        private void Koled_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = sender as TextBox;
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
        private void Float (object sender, TextCompositionEventArgs e) //Правило Float
        {
            TextBox box = sender as TextBox;
            bool flag = false;
            string rule = "1234567890";
            for (int i = 0; i < box.Text.Length; i++)
            {
                if (box.Text[i] == ',')
                {
                    flag = true;
                    break;
                }
                else flag = false;
            }

            if (flag)
            {
                e.Handled = rule.IndexOf(e.Text) < 0;
            }
            else
            {
                e.Handled = (rule + ',').IndexOf(e.Text) < 0;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult qw = MessageBox.Show("Отменить изменения?", "Предупреждение!", MessageBoxButton.YesNo);
            if (qw == MessageBoxResult.Yes)
            {
                if (Util.kod_t == -1)
                {
                    Nazvanie.Text = "";
                    Koled.Text = "";
                    Vesed.Text = "";
                    Srokg.Text = "";
                    edi.SelectedIndex = -1;
                }
                else
                {
                    Get_data();
                    Set_data();
                }
            }
        }
    }
}
