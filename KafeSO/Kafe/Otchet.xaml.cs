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
    /// Логика взаимодействия для Otchet.xaml
    /// </summary>
    public partial class Otchet : Window
    {
        public Otchet()
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DataTime.Content = DateTime.Now.ToString(); };
            timer.Start();

            InitializeComponent();
            Sozdanie();
            Button_Massiv();
        }

        public void Button_Massiv()
        {
            for (int i=0;i<c;i++)
            {
                Button b = new Button();
                b.Tag = i;
                b.Content = Object_Massiv[i].Zagolovok;
                b.Height = 100;
                b.Width = 200;
                b.Margin = new Thickness(10, 10, 10, 10);
                b.Click += new RoutedEventHandler(Button_Click);

                Wrap1.Children.Add(b);
            }
        }

        class Otcheti
        {
            public string Zagolovok;
            public string Shapka;
            public string Zapros;
            public bool type; //true - табличный ; false - скалярный
        }

        int c = 2;
        Otcheti[] Object_Massiv = new Otcheti[2];
        public void Sozdanie()
        {
            Object_Massiv[0] = new Otcheti();
            Object_Massiv[0].Zagolovok = "Расход блюд";
            Object_Massiv[0].Shapka = "|№\t|Название\t\t|Кол\t|Сумма\t|\n----------------------------------------------";
            Object_Massiv[0].type = true;
            Object_Massiv[0].Zapros = "Select ROW_NUMBER() OVER(ORDER BY Z.Nazv_Menu ASC), Z.Nazv_Menu as N'Название' ,Sum(Z.Kol) as N'Количество', Sum(Z.Cena) as N'Сумма' from _Zakaz Z, Zakaz where Zakaz.Kod = Z.Kod_Zakaz and Zakaz.Data_z > convert(datetime, '" + Proxy.Author_Class.Get_Smena() + "', 104) and (Zakaz.Status = 1 or Zakaz.Status = 2) Group by Z.Nazv_Menu";

            Object_Massiv[1] = new Otcheti();
            Object_Massiv[1].Zagolovok = "Выручка";
            Object_Massiv[1].type = false;
            Object_Massiv[1].Zapros = "Select Sum(Z.Cena) as N'Сумма' from _Zakaz Z , Zakaz where Zakaz.Kod = Z.Kod_Zakaz and Zakaz.Data_z > convert(datetime, '" + Proxy.Author_Class.Get_Smena() + "', 104) and Zakaz.Status = 1";
        }

        SqlDataReader dr;
        int sum;
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            List_otch.Text = "";
            sum = 0;

            if (dr != null)
            {
                dr.Close();
            }

            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand com = new SqlCommand(Object_Massiv[Convert.ToInt32(b.Tag)].Zapros, conK);

            List_otch.Text += "----------------------------------------------\n";
            List_otch.Text += String.Format("\t------{0}------\t\n", Object_Massiv[Convert.ToInt32(b.Tag)].Zagolovok);
            List_otch.Text += "----------------------------------------------\n";
            List_otch.Text += "|Смена от: " + Proxy.Author_Class.Get_Smena() + "\n|Выполнил сотрудник: " + Proxy.Author_Class.Get_Privet() + "\n|Дата создания отчёта: " + DateTime.Now.ToString() + "\n";

            if (Object_Massiv[Convert.ToInt32(b.Tag)].type == true)
            {
                List_otch.Text += Object_Massiv[Convert.ToInt32(b.Tag)].Shapka + "\n";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    Print_TextBox((IDataRecord)dr);
                }
                dr.Close();
                List_otch.Text += "----------------------------------------------\n";
                List_otch.Text += String.Format("|Итого: {0}\n", sum);
                List_otch.Text += "----------------------------------------------\n";
            }
            else
            {
                List_otch.Text += "----------------------------------------------\n";
                List_otch.Text += String.Format("|Итого: {0}\n", com.ExecuteScalar().ToString());
                List_otch.Text += "----------------------------------------------\n";
            }
        }

        public void Print_TextBox(IDataRecord record)
        {
            sum += Convert.ToInt32(record[3]);
            string mini = record[1].ToString();
            if (mini.Length < 17)
            {
                mini += "\t";
            }
            else if (mini.Length<15)
            {
                mini += "\t\t\t";
            }
            List_otch.Text += String.Format("|{0}\t|{1}\t|{2}\t|{3}\t|\n", record[0], String.Format("{0}", mini.Remove(21,28)), record[2], record[3]) ;
            if (mini.Trim().Length>21)
            {
                List_otch.Text += String.Format("|\t|{0}\n", mini.Remove(0, 21));
            }
        }

        public void Click_Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
