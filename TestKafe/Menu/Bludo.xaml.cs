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

namespace TestKafe.Menu
{
    /// <summary>
    /// Логика взаимодействия для Bludo.xaml
    /// </summary>
    public partial class Bludo : Window
    {
        bool check;
        Group G = new Group();
        public Bludo(Menu_c m, Group g, bool c)
        {
            InitializeComponent();
            Util.Declare_sm();
            EdCB.ItemsSource = Util.edin;
            Group.Text = g.Nazv.Trim();
            if (c)
            {
                DataContext = Util.sm;
                Util.sm.Menu = m;
                Util.Set_bludo();
                //int index = Util.edin.FindIndex(item => item.kod_ed == Util.sm.Ed.kod_ed);
                //EdCB.SelectedIndex = index;
                Util.Sum_b = Util.Sum_bludo();
                Util.Sum_n = Util.Sum_net();
            }
            DataContext = Util.sm;
            check = c;
            G = g;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void Float(object sender, TextCompositionEventArgs e) //Правило Float
        {
            TextBox box = sender as TextBox;
            bool flag = false;
            string rule = "1234567890";
            for (int i = 0; i < box.Text.Length; i++)
            {
                if (box.Text[i] == '.')
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
                e.Handled = (rule + '.').IndexOf(e.Text) < 0;
            }
        }

        private void Text(object sender, TextCompositionEventArgs e)
        {
            string rule = ",./?\\][{}*&^%$#@!)(№;%:=+-";
            e.Handled = (rule).IndexOf(e.Text) > 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_ingr w = new Add_ingr();
            w.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedIndex < 0)
            {

            }
            else
            {
                try
                {
                    Util.sm.sostav.RemoveAt(Grid.SelectedIndex);
                    Util.Sum_b = Util.Sum_bludo();
                }
                catch
                {
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                if (check)
                {
                    MessageBoxResult mbr = MessageBox.Show("При изменении блюда, который участвовал в заказе, меняется структура рассчёта итогов.\n Продолжить?", "Предупреждение!", MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        Util.Save_bludo(check, Util.sm, G);
                    }
                    else
                    {

                    }
                }
                else
                {
                    Util.Save_bludo(check, Util.sm, G);
                }
                this.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Util.Sum_b = 0;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                Word_otch otch = new Word_otch();
                otch.Bludo(Util.sm);
            }
        }

        private bool Validation()
        {
            if (Util.sm.Menu.Nazv == "" || Util.sm.Menu.Nazv is null || Util.sm.Ed.kod_ed==0 || Util.sm.sostav.Count()==0)
            {
                MessageBox.Show("Введены не все данные!");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
