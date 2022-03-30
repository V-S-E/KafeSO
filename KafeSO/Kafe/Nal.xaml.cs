using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Nal.xaml
    /// </summary>
    public partial class Nal : Window
    {
        public Nal()
        {
            InitializeComponent();
        }

        DataTable T = new DataTable();

        public static bool opl;
        public void Load(object sender, EventArgs e)
        {
            opl = false;
            Itogov.Content = Proxy.Zakaz_Class.Get_Itog();
        }

        private void Close (object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Oplata(object sender, RoutedEventArgs e)
        {
            if (Sdacha.Content.ToString() == "-")
            {
                MessageBox.Show("Денег меньше, чем итог.");
            }
            else
            {
                T = Proxy.Zakaz_Class.Get_DataSet_Zakaz();
                SqlConnection conK = Util.ConnectBD.Get_KDB();
                var Select = T.Select();

                FileInfo file = new FileInfo("Receipt.txt");

                FileStream fc = File.Open(file.FullName, FileMode.Open, FileAccess.ReadWrite);
                fc.SetLength(0);
                fc.Close();

                StreamWriter rec = new StreamWriter(file.FullName);
                rec.WriteLine("-----------Чек заказа-----------\n" 
                    + "-----------Заказ №: "+ Proxy.Zakaz_Class.Get_Kod_Zakaz() +"\t\t\n"
                    + "-----------Дата создания: " + Proxy.Zakaz_Class.Get_DataSet_Zakaz() + "\t\t\n"
                    + "-----------Дата проведения: " + DateTime.Now.ToString() + "\t\t\n"
                    + "-----------Сотрудник: " + Proxy.Author_Class.Get_Privet() + "\t\t\n"
                    + "|№\t|Название\t\t|Кол\t|Сумма\t|\n"
                    + "==============================================\n");

                int i = 0;
                int c = 0;
                foreach (var T in Select)
                {
                    SqlCommand ins = new SqlCommand("insert into Sostav_zakaz(Kod_zak, Kod_Menu, Kol) values (" + Proxy.Zakaz_Class.Get_Kod_Zakaz() + "," + T["Kod_menu"] + "," + T["Kol"] + ")", conK);
                    ins.ExecuteNonQuery();
                    string mini = Convert.ToString(T["Nazv_menu"]);
                    if (mini.Trim().Length < 13)
                    {
                        mini += "\t";
                    }

                    string s = String.Format("|{0}\t|{1}\t|{2}\t|{3}\t|\n", i = i + 1, mini.Remove(21,28), T["Kol"], T["Cena"]);
                    if(mini.Trim().Length>21)
                    {
                        s += String.Format("\t|{0}\t", mini.Remove(0, 21));
                    }
                    c += Convert.ToInt32(T["Cena"]);
                    rec.WriteLine(s);
                }
                rec.WriteLine("Итого: "+ c);
                rec.Close();

                SqlCommand Stat = new SqlCommand("update Zakaz set Status=1 where Kod = " + Proxy.Zakaz_Class.Get_Kod_Zakaz().ToString(), conK);
                Stat.ExecuteNonQuery();
                MessageBox.Show("Оплата проведена. Печатаем чек.\n"+ File.ReadAllText(@"Receipt.txt"));
                this.Close();
                opl = true;
                Menu.z.Close();
                
            }
        }

        private void Izmenenie (object sender, EventArgs e)
        {
            if (Dengi.Text == "" || Convert.ToInt32(Dengi.Text) < Proxy.Zakaz_Class.Get_Itog())
            {
                Sdacha.Content = "-";
            }
            else
            {
                Sdacha.Content = Convert.ToInt32(Dengi.Text) - Proxy.Zakaz_Class.Get_Itog();
            }
        }

        private void Click_Btn(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            Dengi.Text += B.Content;
        }

        private void Click_Del(object sender, RoutedEventArgs e)
        {
            Dengi.Clear();
        }
    }
}
