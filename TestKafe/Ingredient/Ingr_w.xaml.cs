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
    /// Логика взаимодействия для Ingr_w.xaml
    /// </summary>
    public partial class Ingr_w : Window
    {
        public Ingr_w()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.Get_kod_i() == "" || Util.Get_kod_i() is null)
            {
                Vesov.IsChecked = true;
            }
            else
            {
                Select_data();
                Set_data();
                Check_ves();
                Tov.ItemsSource = Util.tovaris;
            }
        }

        static SqlConnection con = Util.Get_k();
        //Переменные для хранения значений полей таблицы ингредиенты
        string nazv_i;
        bool ves;
        bool del;
        float brut;
        float cena;
        int ed;
        public void Select_data()
        {
            SqlCommand s11 = new SqlCommand(@"select Nazv from Ingredient where Kod = " + Util.Get_kod_i(), con);
            nazv_i = s11.ExecuteScalar().ToString();
            SqlCommand s12 = new SqlCommand(@"select Ves from Ingredient where Kod = " + Util.Get_kod_i(), con);
            ves = Convert.ToBoolean(s12.ExecuteScalar());
            SqlCommand s13 = new SqlCommand(@"select Del from Ingredient where Kod = " + Util.Get_kod_i(), con);
            del = Convert.ToBoolean(s13.ExecuteScalar());
            SqlCommand s14 = new SqlCommand(@"select Standart_brut from Ingredient where Kod = " + Util.Get_kod_i(), con);
            brut = Convert.ToSingle(s14.ExecuteScalar());
            SqlCommand s15 = new SqlCommand(@"select Cena from Ingredient where Kod = " + Util.Get_kod_i(), con);
            cena = Convert.ToSingle(s15.ExecuteScalar());
            SqlCommand s16 = new SqlCommand(@"select Kod from ed where Kod = (select ed from Ingredient where Kod =" + Util.Get_kod_i() + ")", con);
            ed = Convert.ToInt32(s16.ExecuteScalar());
        }


        public void Set_data()
        {

            Nazvanie.Text = nazv_i;
            Vesov.IsChecked = ves;
            Delim.IsChecked = del;
            Check_ves();
            Edin.ItemsSource = Util.edin;

            int index = Util.edin.FindIndex(item => item.kod_ed == ed);
            Edin.SelectedIndex = index;
            Vesed.Text = brut.ToString();
            Cenars.Text = cena.ToString();
        }
        public void Check_ves()
        {
            if (Vesov.IsChecked.Value == true)
            {
                Delim_l.Visibility = Visibility.Collapsed;
                Delim.Visibility = Visibility.Collapsed;
                Vesed_l.Visibility = Visibility.Collapsed;
                Vesed.Visibility = Visibility.Collapsed;
                Vesed.Text = "1";
                Delim.IsChecked = true;
            }
            else
            {
                Delim_l.Visibility = Visibility.Visible;
                Delim.Visibility = Visibility.Visible;
                Vesed_l.Visibility = Visibility.Visible;
                Vesed.Visibility = Visibility.Visible;
            }
        }

        private void Vesov_Checked(object sender, RoutedEventArgs e)
        {
            Check_ves();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Clear_razn();
            Util.Clear_tovari();
            Util.Clear_kod_i();
            Util.Clear_tovari();
            Util.Clear_list_ingr();
            Util.Set_list_ingr();
            Util.kod_t = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Util.Get_kod_i() == "" || Util.Get_kod_i() is null)
            {
                MessageBox.Show("Создайте ингредиент.");
            }
            else
            {
                Add_razn w = new Add_razn();
                w.ShowDialog();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Util.Get_kod_i() == "" || Util.Get_kod_i() is null)
            {
                MessageBox.Show("Создайте ингредиент.");
            }
            else
            {
                Util.Delete_razn(Raz.SelectedIndex.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Util.Get_kod_i() == "" || Util.Get_kod_i() is null)
            {
                if (Edin.SelectedIndex == -1 || Nazvanie.Text is null || Cenars.Text is null || Vesed.Text is null || Nazvanie.Text.Trim() == "" || Cenars.Text.Trim() == "" || Vesed.Text.Trim() == "")
                {
                    MessageBox.Show("Введите все данные для записи!");
                }
                else
                {
                    SqlCommand i1 = new SqlCommand(@"insert into Ingredient (Nazv, ed, Cena, Ves, Standart_brut, Del) values (N'" + Nazvanie.Text + "', " + Util.edin[Edin.SelectedIndex].kod_ed + ", @cena, " + Convert.ToInt16(Vesov.IsChecked) + ", @ves , " + Convert.ToInt16(Delim.IsChecked) + ")", con);
                    SqlParameter p1 = new SqlParameter("@cena", Convert.ToSingle(Cenars.Text));
                    SqlParameter p2 = new SqlParameter("@ves", Convert.ToSingle(Vesed.Text));
                    i1.Parameters.Add(p1);
                    i1.Parameters.Add(p2);
                    i1.ExecuteNonQuery();
                    SqlCommand kod = new SqlCommand(@"Select top 1 Kod from Ingredient order by Kod desc", con);
                    Util.Set_kod_i(kod.ExecuteScalar().ToString());
                    MessageBox.Show("Запись успешно добавлена!");
                }
            }
            else
            {
                //Вопрос: Сохранить изменения?

                if (Vesov.IsChecked.Value == true)
                {
                    Vesed.Text = "1";
                    Delim.IsChecked = true;
                }
                SqlCommand u1 = new SqlCommand(@"update Ingredient set Nazv = N'" + Nazvanie.Text + "', ed = " + Util.edin[Edin.SelectedIndex].kod_ed + ", Cena = @cena, Ves = " + Convert.ToInt16(Vesov.IsChecked) + ", Standart_brut = @ves , Del = " + Convert.ToInt16(Delim.IsChecked) + " where kod = " + Util.Get_kod_i(), con);
                SqlParameter p1 = new SqlParameter("@cena", Convert.ToSingle(Cenars.Text));
                SqlParameter p2 = new SqlParameter("@ves", Convert.ToSingle(Vesed.Text));
                u1.Parameters.Add(p1);
                u1.Parameters.Add(p2);
                //MessageBox.Show(u1.CommandText);
                u1.ExecuteNonQuery();
                MessageBox.Show("Данные успешно обновлены!");
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.kod_t = Convert.ToInt32(l.Tag);
            Add_tov w = new Add_tov();
            w.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
                MessageBoxResult qw = MessageBox.Show("Отменить изменения?","Предупреждение!",MessageBoxButton.YesNo);
                if (qw == MessageBoxResult.Yes)
                {
                    if (Util.Get_kod_i() == "" || Util.Get_kod_i() is null)
                    {
                        Nazvanie.Text = "";
                        Cenars.Text = "";
                        Vesov.IsChecked = true;
                    }
                    else
                    {
                        Select_data();
                        Set_data();
                        Check_ves();
                    }
                }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Util.kod_ed_ingr = Util.edin[Edin.SelectedIndex].kod_ed;
            Add_tov w = new Add_tov();
            w.ShowDialog();
        }

        private void Float(object sender, TextCompositionEventArgs e) //Правило Float
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
    }
}
