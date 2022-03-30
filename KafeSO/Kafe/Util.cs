using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace Kafe
{
    class Util
    {
        private static SqlConnection conK;
        static string[] str;
        static FileInfo file;
        static FileInfo file1;
        static StreamWriter zn;
        static FileStream fc;
        public static class Check
        {
            public static string Return_Value()
            {
                return str[0];
            }

            public static void Include_File()
            {
                file = new FileInfo("SqlCon.txt");
                file1 = new FileInfo("Receipt.txt");
                if (System.IO.File.Exists(file.FullName))
                {
                    str = File.ReadAllLines(file.FullName);
                }
                else
                {
                    file.Create();
                    StreamWriter stream = new StreamWriter(file.FullName);
                    stream.WriteLine("**");
                    stream.Close();
                    str = File.ReadAllLines(file.FullName);
                }

                if (System.IO.File.Exists(file1.FullName))
                {

                }
                else
                {
                    file1.Create();
                }
            }

            public static void Set_ConStr(string k)// k = строка подключения kafe
            {
                Clear_File();
                zn = file.AppendText();
                zn.WriteLine(k);
                zn.Close();
            }


            public static bool Check_File()
            {
                if (file.Length <= 3)
                {
                    return true;
                }
                return false;
            }

            public static void Clear_File()
            {
                fc = File.Open(file.ToString(), FileMode.Open, FileAccess.ReadWrite);
                fc.SetLength(0);
                fc.Close();
            }
        }

        public class ConnectBD
        {

            public static SqlConnection ConnectKDB()
            {
                Util.Check.Include_File();
                try
                {
                    conK = new SqlConnection(str[0]);
                    return conK;
                }
                catch
                {
                    MessageBox.Show("Проверьте правильность ввода строки!");
                    return null;
                }
            }


            public static SqlConnection Get_KDB()
            {
                return conK;
            }

        }

        public class Windows
        {
            public static Setting s = new Setting();
            public static Authorization a = new Authorization();
            public static Load l = new Load();
            public static Menu m = new Menu();
        } 
    }
}
