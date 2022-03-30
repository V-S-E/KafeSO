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

namespace TestKafe.Menu
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Util.Set_trees();
            LI.ItemsSource = Util.trees;
        }
        public Menu_c m;
        public Tree t;
        public Group g;
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            m = (Menu_c)Bl.SelectedItem;
            t = (Tree)LI.SelectedItem;
            g = t.G_g;
            Bludo w = new Bludo(m, g, true);
            w.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m = new Menu_c();
                t = (Tree)LI.SelectedItem;
                g = t.G_g;
                Bludo w = new Bludo(m, g, false);
                w.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не выбрана группа!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Вы действительно хотите удалить выбранное блюдо?", "Предупреждение!", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                Menu_c menu = (Menu_c)Bl.SelectedValue;
                Util.Delete_bludo(menu.Kod);
                Util.Set_trees();
                LI.ItemsSource = Util.trees;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Util.Set_trees();
            LI.ItemsSource = Util.trees;
        }
    }
}
