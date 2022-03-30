using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestKafe.Settings
{
    /// <summary>
    /// Логика взаимодействия для Sett.xaml
    /// </summary>
    public partial class Sett : Window
    {
        Main ma;
        bool flag;
        public Sett(Main m)
        {
            InitializeComponent();
            PathOtch.Text = Util.Get_otch();
            BD.Text = Util.Get_con();
            ma = m;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //System.Windows.MessageBox.Show(FBD.SelectedPath);
                Util.Set_otch(FBD.SelectedPath);
                PathOtch.Text = FBD.SelectedPath;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            flag = true;
            Util.Close_con();
            ma.Close();
            Util.Set_con(BD.Text);
            if (Util.Access(Log.Text, Pass.Password))
            {
                System.Windows.MessageBox.Show("При подключении произошла ошибка. Проврьте данные или настройки.");
            }
            else
            {
                System.Windows.MessageBox.Show("Подключение открыто.");
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (flag)
            {
                Main main = new Main();
                this.Close();
                main.Show();
            }
            else
            {
                this.Close();
            }
        }
    }
}
