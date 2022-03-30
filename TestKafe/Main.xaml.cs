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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace TestKafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            //MessageBox.Show(Util.con);
        }


        bool fm = false;
        public void Click_menu(object sender, RoutedEventArgs e)
        {
            if (fm == false)
            {
                Menu_g.Width = 250;
                fm = true;
            }
            else
            {
                Menu_g.Width = 50;
                fm = false;
            }
        }

        Ingredient.List_ingr I;
        Glav.Glav G;
        Postavka.TabPost P;
        Sklad.Sklad S;
        Menu.Menu M;
        Settings.Sett Sett;

        public void Click_RBTN(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string tag = rb.Tag.ToString();
            switch (tag)
            {
                case "1":
                    { 
                        I = new Ingredient.List_ingr();
                        Frame.Content = I;
                        break; 
                    }
                case "2":
                    {
                        G = new Glav.Glav();
                        Frame.Content = G;
                        break; 
                    }
                case "3":
                    {
                        P = new Postavka.TabPost();
                        Frame.Content = P;
                        break;
                    }
                case "4":
                    {
                        S = new Sklad.Sklad();
                        Frame.Content = S;
                        break;
                    }
                case "5":
                    {
                        M = new Menu.Menu();
                        Frame.Content = M;
                        break;
                    }
                case "6":
                    {
                        Sett = new Settings.Sett(this);
                        Sett.ShowDialog();
                        break;
                    }
            }
        }

        public void Load(object sender, EventArgs e)
        {
            MainFrame.IsChecked = true;
            Frame.Content = new Glav.Glav();
            Util.Set_edin();
            Util.Set_prich_sp();
            Util.kod_t = -1;
        }
    }
}
