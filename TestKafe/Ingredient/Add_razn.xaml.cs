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
    /// Логика взаимодействия для Add_razn.xaml
    /// </summary>
    public partial class Add_razn : Window
    {
        public Add_razn()
        {
            InitializeComponent();
        }

        SqlConnection con = Util.Get_k();
        private void Click_add(object sender, RoutedEventArgs e)
        {
            SqlCommand s1 = new SqlCommand("insert into Raznovidnosti_ingr(Kod_ingr, Nazv) values (" + Util.Get_kod_i() + " , N'" + Nazv.Text + "')", con);
            s1.ExecuteNonQuery();
            Util.Clear_razn();
            Util.Set_razn();
            this.Close();
        }
        private void Click_otm(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
