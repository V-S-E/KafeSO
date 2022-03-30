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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Load.xaml
    /// </summary>
    public partial class Load : Window
    {
        public Load()
        {
            InitializeComponent();
            Load_Form();
        }

        public void Click_ok(object sender, RoutedEventArgs e)
        {
            this.Close();
            Authorization a = new Authorization();
            a.Show();
        }

        bool p;
        private void Load_Form()
        {
            Util.Check.Include_File();

            Label l3 = new Label();
            l3.Foreground = new SolidColorBrush(Colors.White);
            l3.Content = "Подключение к файлу прошло успешно.";
            Lod.Children.Add(l3);

            p = Util.Check.Check_File();

            if (p==true)
            {
                this.Close();
                Util.Windows.s.Show();
            }
            else
            {
                Label l2 = new Label();
                l2.Foreground = new SolidColorBrush(Colors.White);
                l2.Content = "Файл прошёл проверку.";
                Lod.Children.Add(l2);
            }

            try
            {
                SqlConnection conK = Util.ConnectBD.ConnectKDB();

                conK.Open();

                if (conK.State == System.Data.ConnectionState.Open)
                {
                    for (int i=0; i<3; i++)
                    {
                        Label l1 = new Label();
                        l1.Foreground = new SolidColorBrush(Colors.White);
                        switch (i)
                        {
                            case 0:
                                l1.Content = "Подключение открыто.";
                                Lod.Children.Add(l1);
                                break;
                            case 1:
                                l1.Content = "Статус подключение к базе \"Kafe\" " + conK.State.ToString();
                                Lod.Children.Add(l1);
                                break;
                            case 2:
                                Ok_but.Visibility = Visibility.Visible;
                                break;
                        }
                    }
                }
            }
            catch
            {
                this.Close();
                Util.Windows.s.Show();
            }
        }
    }
}
