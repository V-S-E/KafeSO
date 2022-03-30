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
using System.Data;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Redakt.xaml
    /// </summary>
    public partial class Redakt : Window
    {
        public Redakt()
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DataTime.Content = DateTime.Now.ToString(); };
            timer.Start();

            InitializeComponent();
            Button_Massiv();
        }


        //SqlDataReader dat;
        //SqlDataReader Read_Zakazi;
        string tags;
        public void Button_Massiv()
        {
            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand Zakazi = new SqlCommand("select Z.Kod, S.Nazv, Sum(Zz.Cena) as Cena, Z.Status from _Zakaz Zz, Zakaz Z, Status S where Z.Kod = Zz.Kod_Zakaz and S.Kod = Z.Status and Z.Data_z > convert(datetime, '"+ Proxy.Author_Class.Get_Smena() + "', 104) Group by Z.Kod, S.Nazv, Z.Status Order by Z.Kod", conK);


            SqlDataReader Read_Zakazi = Zakazi.ExecuteReader();

            if (Read_Zakazi.HasRows)
            {
                while (Read_Zakazi.Read())
                {
                    Button B = new Button();
                    StackPanel S = new StackPanel();
                    Label l1 = new Label();
                    Label l2 = new Label();
                    Label l3 = new Label();

                    Object kod = Read_Zakazi.GetValue(0);
                    Object stat = Read_Zakazi.GetValue(1);
                    Object cena = Read_Zakazi.GetValue(2);
                    Object kod_s = Read_Zakazi.GetValue(3);

                    l1.Content = "Заказ №" + kod.ToString();
                    l2.Content = "Статус: " + stat.ToString();
                    l3.Content = "Сумма: " + cena.ToString();

                    S.Children.Add(l1);
                    S.Children.Add(l2);
                    S.Children.Add(l3);

                    B.Height = 100;
                    B.Width = 200;
                    B.Margin = new Thickness(10, 10, 10, 10);
                    B.Content = S;
                    B.Tag = Convert.ToInt32(kod);
                    B.Click += new RoutedEventHandler(Click_Button);

                    switch (kod_s.ToString())
                    {
                        case "1" :
                            B.Background = new SolidColorBrush(Colors.LightGreen);
                            break;
                        case "2":
                            B.Background = new SolidColorBrush(Colors.PaleVioletRed);
                            break;
                        case "4":
                            B.Background = new SolidColorBrush(Colors.Moccasin);
                            break;
                    }

                    Wrap1.Children.Add(B);
                }
            }
            else
            {
                Label l = new Label();
                l.Content = "Нет заказов";
                l.HorizontalAlignment = HorizontalAlignment.Center;
                l.VerticalAlignment = VerticalAlignment.Center;
                l.FontSize = 50;
                l.Foreground = new SolidColorBrush(Colors.White);
                grid.Children.Add(l);
            }
            Read_Zakazi.Close();
        }

        public void Click_Button(object sender, RoutedEventArgs e)
        {

            Button B = (Button)sender;
            tags = B.Tag.ToString();
            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand Table_Sostav = new SqlCommand("select Z.Nazv_Menu, Z.Kol, Z.Cena from _Zakaz Z where Z.Kod_Zakaz = "+tags, conK);
            SqlCommand itog = new SqlCommand("select Sum(Z.Cena) from _Zakaz Z where Z.Kod_Zakaz = " + tags, conK);
            Itog.Content = itog.ExecuteScalar().ToString();
            SqlCommand get_kod_sotr = new SqlCommand("Select Kod_Sotr from Zakaz where Kod = " + tags, conK);
            string kod_sotr = get_kod_sotr.ExecuteScalar().ToString();
            SqlCommand sotr = new SqlCommand("Select W.Privet from Worker W where W.Kod = " + kod_sotr, conK);
            Sotr.Content = sotr.ExecuteScalar();
            SqlCommand date1 = new SqlCommand("Select Data_z from Zakaz where Kod = "+tags, conK);
            Date1.Content = date1.ExecuteScalar().ToString();

            SqlDataAdapter ad = new SqlDataAdapter(Table_Sostav);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            List_sostav.ItemsSource = ds.Tables[0].DefaultView;

        }

        public void Click_Otm1(object sender, RoutedEventArgs e)
        {
            if (tags != null || List_sostav.SelectedValue != null)
            {
                SqlConnection conK = Util.ConnectBD.Get_KDB();
                SqlCommand com = new SqlCommand("update Zakaz set Status = 2 where Kod = "+tags+" and Status=1", conK);
                com.ExecuteNonQuery();
                MessageBox.Show("Обновление статуса успешно!");
                Wrap1.Children.Clear();
                Button_Massiv();
            }
        }

        public void Click_Otm2(object sender, RoutedEventArgs e)
        {
            if (tags != null || List_sostav.SelectedValue != null)
            {
                SqlConnection conK = Util.ConnectBD.Get_KDB();
                SqlCommand com = new SqlCommand("update Zakaz set Status = 4 where Kod = " + tags + " and Status=1", conK);
                com.ExecuteNonQuery();
                MessageBox.Show("Обновление статуса успешно!");
                Wrap1.Children.Clear();
                Button_Massiv();
            }
        }

        public void Click_Back (object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
