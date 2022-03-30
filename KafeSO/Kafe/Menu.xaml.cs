using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Load_Form(Object sender, EventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { Date.Content = DateTime.Now.ToShortDateString(); Clock.Content = DateTime.Now.ToShortTimeString(); };
            timer.Start();

            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand Get_Smena = new SqlCommand("Select top 1 value from Log_Journal where Tip = 1 order by kod desc", conK);
            SqlCommand Smena_Open_Date = new SqlCommand("Select top 1 Data_log from Log_Journal where Tip = 1 and Value = 'Open' order by kod desc", conK);
            SqlCommand Insert_Smena = new SqlCommand("insert into Log_Journal(Data_log, Sotr, Tip, Value) values (GetDate(), "+ Proxy.Author_Class.Get_Kod_Sotr().ToString() +", 1, 'Open')", conK);
            Privetstvie.Content = Proxy.Author_Class.Get_Privet();


            if (Get_Smena.ExecuteScalar() == null || Get_Smena.ExecuteScalar().ToString() == "Close")
            {
                CloseSmena.Visibility = Visibility.Collapsed;
                MessageBoxResult result = MessageBox.Show("Смена закрыта. Открыть?", "Смена", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Insert_Smena.ExecuteNonQuery();
                    MessageBox.Show("Смена открыта!");
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Close();
                    MessageBox.Show("Для работы приложения нужно открыть смену!");
                    Application.Current.Shutdown();
                }
            }

            Proxy.Author_Class.Set_Smena(Smena_Open_Date.ExecuteScalar().ToString());

            if (Get_Smena.ExecuteScalar().ToString() == "Open")
            {
                CloseSmena.Visibility = Visibility.Visible;
                Smena_S.Content = Proxy.Author_Class.Get_Smena();
            }

            Vir();
        }

        private void Click_Close_Smena(Object sender, EventArgs e)
        {
            SqlConnection conA = Util.ConnectBD.Get_KDB();
            SqlCommand Smena = new SqlCommand("insert into Log_Journal(Data_log, Sotr, Tip, Value) values (GetDate()," + Proxy.Author_Class.Get_Kod_Sotr().ToString() + ", 1, 'Close')", conA);
            SqlCommand Date_Close_Smena = new SqlCommand("Select top 1 Data_Log from Log_Journal where Tip = 1 order by kod desc", conA);
            Smena.ExecuteNonQuery();
            this.Close();
            MessageBox.Show("Смена закрыта c датой " + Date_Close_Smena.ExecuteScalar().ToString());
            Application.Current.Shutdown();
        }

        public static Zakaz z;
        private void New_Zakaz(Object sender, EventArgs e)
        {
            this.Hide();
            z = new Zakaz();
            z.Show();
        }

        private void Redakt (Object sender, EventArgs e)
        {
            //this.Hide();
            Redakt r = new Redakt();
            r.ShowDialog();
        }

        public void Click_Block(object sender, RoutedEventArgs e)
        {
            Authorization a = new Authorization();
            a.Show();
            this.Close();
        }

        public void Otch(object sender, RoutedEventArgs e)
        {
            Otchet o = new Otchet();
            o.ShowDialog();
        }

        public void Vir()
        {
           SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand s1 = new SqlCommand("Select count(Z.Kod) from Zakaz Z where Z.Data_z > convert(datetime, '" + Proxy.Author_Class.Get_Smena() + "', 104) and Z.Status = 1", conK);
            Kol_zak.Content = s1.ExecuteScalar();

            SqlCommand s2 = new SqlCommand("Select Sum(Z.Cena) as N'Сумма' from _Zakaz Z , Zakaz where Zakaz.Kod = Z.Kod_Zakaz and Zakaz.Data_z > convert(datetime, '" + Proxy.Author_Class.Get_Smena() + "', 104) and Zakaz.Status = 1", conK);
            Viruchka.Content = s2.ExecuteScalar();
        }
    }
}
