using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Kafe
{
    class Proxy
    {
        public static class Author_Class
        {
            private static string Privet;
            private static int Kod_Sotr;
            private static string Smena;

            public static void Set_Privet(string p)
            {
                Privet = p;
            }
            public static string Get_Privet()
            {
                return Privet;
            }
            public static void Clear_Privet()
            {
                Privet = null;
            }

            public static void Set_Kod_Sotr(int k)
            {
                Kod_Sotr = k;
            }
            public static int Get_Kod_Sotr()
            {
                return Kod_Sotr;
            }
            public static void Clear_Kod_Sotr()
            {
                Kod_Sotr = 0;
            }

            public static void Set_Smena(string s)
            {
                Smena = s;
            }
            public static string Get_Smena()
            {
                return Smena;
            }
            public static void Clear_Smena()
            {
                Smena = null;
            }
        }

        public static class Zakaz_Class
        {
            private static DataTable Zakaz;
            private static int Kod_Zak;
            private static int Itog;
            public static void Set_DataSet_Zakaz(DataTable D)
            {
                Zakaz = D;
            }
            public static DataTable Get_DataSet_Zakaz()
            {
                return Zakaz;
            }

            public static void Set_Kod_Zakaz(int Kod)
            {
                Kod_Zak = Kod;
            }
            public static int Get_Kod_Zakaz()
            {
                return Kod_Zak;
            }

            public static void Set_Itog (int it)
            {
                Itog = it;
            }
            public static int Get_Itog()
            {
                return Itog;
            }
        }

    }
}
