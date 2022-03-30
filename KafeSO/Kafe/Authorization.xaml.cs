using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        public void Load(Object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DateNow.Content = DateTime.Now.ToShortDateString(); TimeNow.Content = DateTime.Now.ToShortTimeString(); };
            timer.Start();
        }

        public DateTime timeL;
        public DateTime timeP;
        public void Focus_Log(object sender, RoutedEventArgs e)
        {
            DateTime time = new DateTime();
            if (Log.IsFocused)
            {
                time = DateTime.Now;
            }
            timeL = time;
        }
        public void Focus_Pas(object sender, RoutedEventArgs e)
        {
            DateTime time = new DateTime();
            if (Pas.IsFocused)
            {
                time = DateTime.Now;
            }
            timeP = time;
        }


        private void Cl_tl(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;

            if (timeL > timeP)
            {
                Log.Text += B.Content;
            }
            else
            {
                Pas.Password += B.Content;
            }
        }

        private void Del_Cl(object sender, RoutedEventArgs e)
        {
            if (timeL > timeP)
            {
                Log.Clear();
            }
            else
            {
                Pas.Clear();
            }
        }

        private void Click_Access(object sender, RoutedEventArgs e)
        {
            if (Log.Text == "")
            {
                MessageBox.Show("Введите логин");
            }
            else
            {
                if (Pas.Password == "")
                {
                    MessageBox.Show("Введите пароль");
                }
                else
                {
                    SqlConnection conK = Util.ConnectBD.Get_KDB();
                    SqlParameter Login1 = new SqlParameter("@Login", Log.Text);
                    SqlParameter Password1 = new SqlParameter("@Password", Pas.Password);
                    SqlParameter Login2 = new SqlParameter("@Login", Log.Text);
                    SqlParameter Password2 = new SqlParameter("@Password", Pas.Password);
                    string SelectP = "SELECT Privet from Worker WHERE Kod = (SELECT kod_work FROM ACCESS where Access.Login = @Login AND Access.Password = @Password)";
                    string SelectK = "SELECT Kod from Worker WHERE Kod = (SELECT kod_work FROM ACCESS where Access.Login = @Login AND Access.Password = @Password)";
                    SqlCommand access = new SqlCommand(SelectP, conK);
                    SqlCommand kod = new SqlCommand(SelectK, conK);
                    kod.Parameters.Add(Login1);
                    kod.Parameters.Add(Password1);
                    access.Parameters.Add(Login2);
                    access.Parameters.Add(Password2);

                    var check = access.ExecuteScalar();

                    if (check == null)
                    {
                        MessageBox.Show("Неверные логин или пароль!");
                    }
                    else
                    {
                        string pr = access.ExecuteScalar().ToString();
                        int kod_s = Convert.ToInt32(kod.ExecuteScalar());

                        Proxy.Author_Class.Set_Kod_Sotr(kod_s);
                        Proxy.Author_Class.Set_Privet(pr);
                        Menu m = new Menu();
                        this.Close();
                        m.Show();
                        this.Close();
                    }
                }
            }
        }

        public void Click_Setting(Object sender, RoutedEventArgs e)
        {
            Setting s = new Setting();
            this.Close();
            s.ShowDialog();
        }

        public void Down_system(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
