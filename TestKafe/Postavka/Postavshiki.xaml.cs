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

namespace TestKafe.Postavka
{
    /// <summary>
    /// Логика взаимодействия для Postavshiki.xaml
    /// </summary>
    public partial class Postavshiki : Page
    {
        public Postavshiki()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Util.postavkis.Count() > 0)
            {
                Util.Clear_postav_add();
            }
            Util.Set_postav_add();
            LI.ItemsSource = Util.add_post;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Util.poisk = TextP.Text;
            Util.Clear_postav_add();
            Util.Set_postav_add();
            Util.poisk = "";
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label l = sender as Label;
            Util.p.Kod = (int)l.Tag;
            Postavka.Info_postav w = new Postavka.Info_postav();
            w.ShowDialog();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Util.p.Kod = 0;
            Postavka.Info_postav w = new Postavka.Info_postav();
            w.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (LI.SelectedIndex < 0)
            {

            }
            else
            {
                MessageBoxResult qw = MessageBox.Show("Удалить поставщика \"" + Util.add_post[LI.SelectedIndex].nazv.Trim() + "\"?", "Предупреждение!", MessageBoxButton.YesNo);
                if (qw == MessageBoxResult.Yes)
                {
                    if (Util.Delete_post_info(Util.add_post[LI.SelectedIndex].kod))
                    {
                        MessageBox.Show("Данные удалены!");
                    }
                    else
                    {
                        MessageBox.Show("При удалении произошла ошибка.");
                    }
                }
            }
        }
    }
}
