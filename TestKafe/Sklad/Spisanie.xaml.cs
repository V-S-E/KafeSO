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

namespace TestKafe.Sklad
{
    /// <summary>
    /// Логика взаимодействия для Spisanie.xaml
    /// </summary>
    public partial class Spisanie : Window
    {
        public Spisanie()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Util.index_sp == -1)
            {
                DataContext = Util.Sp_add;
                Util.Initialize_sp_add();
                Prich_text.Visibility = Visibility.Collapsed;
                Prich_combo.ItemsSource = Util.prichina_oc;
                Util.Sp_add.Data = DateTime.Now;
            }
            else
            {
                DataContext = Util.sp[Util.index_sp];
                Prich_combo.Visibility = Visibility.Collapsed;
                Kolich.IsReadOnly = true;
                Com.IsReadOnly = true;
                Date_d.IsManipulationEnabled = false;
                Save.Visibility = Visibility.Collapsed;
                Otm.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.index_sp = -1;
            Util.Sp_add.Comment = "";
            
            Util.Sp_add.Kol = 0;
            Util.Clear_sp();
            Util.Set_sp();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Util.Save_sp())
            {
                MessageBox.Show("Не удалось сохранить списание. Проверьте данные.");
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Add_tov_spis w = new Add_tov_spis();
            w.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Add_sotr w = new Add_sotr();
            w.ShowDialog();
        }

        private void Float(object sender, TextCompositionEventArgs e) //Правило Float
        {
            TextBox box = sender as TextBox;
            bool flag = false;
            string rule = "1234567890";
            for (int i = 0; i < box.Text.Length; i++)
            {
                if (box.Text[i] == ',')
                {
                    flag = true;
                    break;
                }
                else flag = false;
            }

            if (flag)
            {
                e.Handled = rule.IndexOf(e.Text) < 0;
            }
            else
            {
                e.Handled = (rule + ',').IndexOf(e.Text) < 0;
            }
        }
    }
}
