using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace Kafe
{
    /// <summary>
    /// Логика взаимодействия для Zakaz.xaml
    /// </summary>
    public partial class Zakaz : Window
    {
        public Zakaz()
        {
            InitializeComponent();
        }

        int kod_z;
        int grop;

        DataSet set;
        DataTable Sostav;

        private void Load_Form(object sender, EventArgs e)
        {
            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlParameter Kod_Sotr = new SqlParameter("@Kod_S", Proxy.Author_Class.Get_Kod_Sotr());
            SqlCommand Insert_Zakaz = new SqlCommand("INSERT INTO Zakaz (Status, Kod_Sotr) values (5, @Kod_S)", conK);
            Insert_Zakaz.Parameters.Add(Kod_Sotr);
            SqlCommand Get_Kod_Zakaz = new SqlCommand("SELECT IDENT_CURRENT('Zakaz')", conK);
            Insert_Zakaz.ExecuteNonQuery();
            kod_z = Convert.ToInt32(Get_Kod_Zakaz.ExecuteScalar());
            Proxy.Zakaz_Class.Set_Kod_Zakaz(kod_z);

            set = new DataSet("Set_Sostav");
            Sostav = new DataTable("Sostav");
            Sostav.Columns.Add("Kod_menu", Type.GetType("System.Int32"));
            Sostav.Columns.Add("Nazv_menu", Type.GetType("System.String"));
            Sostav.Columns.Add("Kol", Type.GetType("System.Int32"));
            Sostav.Columns.Add("Cena", Type.GetType("System.Single"));
            set.Tables.Add(Sostav);

            Button_Massiv(1);

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { DataTime.Content = DateTime.Now.ToString();};
            timer.Start();
        }
        //не менять
        public void Close_Form(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Nal.opl == true)
            {
                Menu m = new Menu();
                m.Show();
                //Util.Windows.m.Show();
                e.Cancel = false;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Хотите отменить заказ?", "Заказ", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SqlConnection conK = Util.ConnectBD.Get_KDB();
                    SqlCommand Update_Zakaz = new SqlCommand("update Zakaz set Status = 6 where Kod = " + kod_z.ToString(), conK);
                    Update_Zakaz.ExecuteNonQuery();
                    Util.Windows.m.Show();
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        //не менять
        private void Click_Close(object sender, EventArgs e)
        {
                this.Close();
        }

        public void Button_Massiv(int gr)
        {
            grop = gr;

            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlParameter Group = new SqlParameter("@Group", gr);
            SqlCommand Button_Name = new SqlCommand("Select * from dbo.Button(@Group) as t2 where t2.Vid=N'G'"+
            "union Select * from dbo.Button(@Group) as t1 where exists(Select * from Menu where Rem = 0 and t1.Kod = Kod and t1.Vid = N'M')", conK);
            Button_Name.Parameters.Add(Group);
            SqlDataReader Button_Read = Button_Name.ExecuteReader();

            if (Button_Read.HasRows)
            {
                while (Button_Read.Read())
                {
                    Button B = new Button();
                    B.Height = 100;
                    B.Width = 100;
                    B.Margin = new Thickness(10, 10, 0, 10);
                    B.HorizontalAlignment = HorizontalAlignment.Center;

                    Object Element = Button_Read.GetValue(0);
                    Object Kod_obj = Button_Read.GetValue(1);
                    Object Vid = Button_Read.GetValue(2);
                    TextBlock t = new TextBlock();
                    t.TextWrapping = TextWrapping.Wrap;
                    t.TextAlignment = TextAlignment.Justify;
                    t.VerticalAlignment = VerticalAlignment.Center;
                    t.HorizontalAlignment = HorizontalAlignment.Center;
                    t.Width = 90;
                    t.Height = 90;
                    t.Text = Element.ToString();
                    B.Content = t;

                    if (Vid.ToString() == "G")
                    {
                        if (Convert.ToInt32(Kod_obj) != 1)
                        {
                            B.Tag = Convert.ToInt32(Kod_obj);
                            B.Background = new SolidColorBrush(Colors.AliceBlue);
                            B.Click += new RoutedEventHandler(Mouse_Click_Group);
                            Wrap.Children.Add(B);
                        }
                    }
                    else if (Vid.ToString() == "M")
                    {
                        B.Background = new SolidColorBrush(Colors.LightYellow);
                        B.Tag = Convert.ToInt32(Kod_obj);
                        B.Click += new RoutedEventHandler(Mouse_Click_Menu);
                        Wrap.Children.Add(B);
                    }
                }
            }
            Button_Read.Close();
        }

        public void Mouse_Click_Group(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            Wrap.Children.Clear();
            Button_Massiv(Convert.ToInt32(B.Tag));
        }

        public void Mouse_Click_Menu(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlCommand Cena = new SqlCommand("Select Cena from _Menu where Kod_Menu = " + B.Tag, conK);
            SqlCommand Nazv = new SqlCommand("Select Nazv_Menu from _Menu where Kod_Menu = " + B.Tag, conK);
            float cena = Convert.ToSingle(Cena.ExecuteScalar());
            string nazv = Nazv.ExecuteScalar().ToString();
            //Проверка на наличие номенклатуры в DataTable

            SqlCommand s1 = new SqlCommand("Select dbo.avail_menu(" + B.Tag + ")", conK);
            bool chk = Convert.ToBoolean(s1.ExecuteScalar());

            if (chk)
            {
                MessageBoxResult mbr = MessageBox.Show("На складе не обнаружено некоторых ингредиентов. Вы действительно хотите добавить блюдо в заказ?", "Предупреждение!", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {

                    DataRow[] row = Sostav.Select("Kod_menu=" + B.Tag);//Попытка получить нужную строку, а после запустить цикл foreach
                    if (row.Count() == 0)
                    {
                        Sostav.Rows.Add(new object[] { B.Tag, nazv, 1, cena });
                    }
                    else
                    {
                        foreach (var r in row)
                        {
                            int i = Convert.ToInt32(r["Kol"]);
                            int c = Convert.ToInt32(r["Cena"]);

                            r["Kol"] = i + 1;
                            r["Cena"] = c + c/i;

                        }
                    }
                    List_Zalkaz();
                }
                else
                {

                }
            }
            else
            {
                DataRow[] row = Sostav.Select("Kod_menu=" + B.Tag);//Попытка получить нужную строку, а после запустить цикл foreach
                if (row.Count() == 0)
                {
                    Sostav.Rows.Add(new object[] { B.Tag, nazv, 1, cena });
                }
                else
                {
                    foreach (var r in row)
                    {
                        int i = Convert.ToInt32(r["Kol"]);
                        int c = Convert.ToInt32(r["Cena"]);

                        r["Kol"] = i + 1;
                        r["Cena"] = c + c / i;

                    }
                }
                List_Zalkaz();
            }
        }

        public void Click_Back(object sender, RoutedEventArgs e)
        {
            SqlConnection conK = Util.ConnectBD.Get_KDB();
            SqlParameter Group = new SqlParameter("@Group", grop);
            SqlCommand Button_Back = new SqlCommand("select Kod_Rod from Tree where Kod_Group = @Group", conK);
            Button_Back.Parameters.Add(Group);
            int p = Convert.ToInt32(Button_Back.ExecuteScalar());
            
            Wrap.Children.Clear();
            Button_Massiv(p);
        }

        public void List_Zalkaz()
        {
            Zakaz_Menu.ItemsSource = Sostav.DefaultView;
            Zakaz_Menu.Columns[0].Visibility = Visibility.Collapsed;
            Zakaz_Menu.Columns[1].Header = "Блюдо";
            Zakaz_Menu.Columns[2].Header = "Количество";
            Zakaz_Menu.Columns[3].Header = "Цена";
            Cena();
        }

        public void Cena()
        {
            object sum = Sostav.Compute("sum(Cena)","");
            Itog.Content = sum.ToString();
        }

        public void Click_Del(object sender, RoutedEventArgs e)
        {
            if (NumBox.Text == "")
            {
                if (Zakaz_Menu.SelectedItem != null)
                {
                    var i = Zakaz_Menu.SelectedIndex;
                    var row = Sostav.Rows[i];
                    row.Delete();
                    List_Zalkaz();
                }
            }
            else
            {
                NumBox.Clear();
            }
        }

        private void Click_Btn(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            if (NumBox.Text == "" && B.Content.ToString() == "0")
            {

            }
            else
            {
                NumBox.Text += B.Content;
            }
        }

        private void Click_Row(object sender, RoutedEventArgs e)
        {
            DataGrid D = (DataGrid)sender;
        }

        public void Click_Nal(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(Itog.Content) == 0)
            {
                MessageBox.Show("Заказ не содержит блюд");
            }
            else
            {
                Proxy.Zakaz_Class.Set_DataSet_Zakaz(Sostav);
                Proxy.Zakaz_Class.Set_Itog(Convert.ToInt32(Itog.Content));
                Nal nal = new Nal();
                nal.ShowDialog();
            }
        }

        private void Click_Plus(object sender, RoutedEventArgs e)
        {
            if (Zakaz_Menu.SelectedItem != null)
            {
                var i = Zakaz_Menu.SelectedIndex;
                DataRow row = Sostav.Rows[i];
                int col = Convert.ToInt32(row["Kol"]);
                int cen = Convert.ToInt32(row["Cena"]);
                string box = NumBox.Text;
                if (box == "")
                {
                    row["Cena"] = cen + cen / col;
                    row["Kol"] = col + 1;
                }
                else
                {
                    row["Cena"] = (cen / col)* Convert.ToInt32(box);
                    row["Kol"] = Convert.ToInt32(box);
                }
                NumBox.Clear();
                List_Zalkaz();
            }
        }

        private void Click_Minus(object sender, RoutedEventArgs e)
        {
            if (Zakaz_Menu.SelectedItem != null)
            {
                var i = Zakaz_Menu.SelectedIndex;
                DataRow row = Sostav.Rows[i];
                int col = Convert.ToInt32(row["Kol"]);
                int cen = Convert.ToInt32(row["Cena"]);
                if (col > 1)
                {
                    row["Cena"] = cen - cen / col;
                    row["Kol"] = col - 1;
                }
                    NumBox.Clear();
                    List_Zalkaz();
            }
        }

        private void Click_Main(object sender, RoutedEventArgs e)
        {
            Wrap.Children.Clear();
            Button_Massiv(1);
        }
    }
}
