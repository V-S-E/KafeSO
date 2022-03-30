using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace TestKafe
{
    class Util
    {
        /// <summary>
        /// Для строки подключения
        /// </summary>

        private static string con = ""; 
        private static SqlConnection conK = new SqlConnection();
        public static SqlConnection Get_k()
        {
            return conK;
        }

        //Для папки с отчётами
        private static string otch = "";
        public static string Get_otch()
        {
            return otch;
        }
        public static string Get_con()
        {
            return con;
        }
        public static void Set_otch(string s)
        {
            if (FileCheck())
            {
                string str = File.ReadAllText(file.FullName);
                str = Regex.Replace(str, @"(?<=<path_o>)(.*)(?=</path_o>)", s);
                File.WriteAllText(file.FullName, str);
            }
            FileCheck();
        }//Запись в файл
        public static void Set_con(string s)
        {
            if (FileCheck())
            {
                string str = File.ReadAllText(file.FullName);
                str = Regex.Replace(str, @"(?<=<con>)(.*)(?=</con>)", s);
                File.WriteAllText(file.FullName, str);
            }
            FileCheck();
        }//Запись в файл
        public static bool Connect(string connection)
        {
            try
            {
                conK.ConnectionString = connection;
                conK.Open();
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static bool Access(string log, string pass)
        {
            string conk = con + "User ID="+log+"; Password ="+pass;
            if(Connect(conk))
            {
                return true;
            }
            else
            {
                return false;
            }
        }//открывает подключение
        public static void Close_con()
        {
            conK.Close();
        }

        //Для файла настроек
        static FileInfo file = new FileInfo("Settings.txt");
        public static bool FileCheck()
        {
            string str;
            if (File.Exists(file.FullName))
            {
                str = File.ReadAllText(file.FullName);
            }
            else
            {
                file.Create();
                StreamWriter stream = new StreamWriter(file.FullName);
                stream.WriteLine("<path_o>" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "</path_o>");
                stream.WriteLine("<con>" + "</con>");
                stream.Close();
                str = File.ReadAllText(file.FullName);
            }
            otch = Regex.Match(str, @"(?<=<path_o>)(.*)(?=</path_o>)").Value;
            con = Regex.Match(str, @"(?<=<con>)(.*)(?=</con>)").Value;
            conK = new SqlConnection(con);
            return true;
        }

        //Переменная для хранения аргумента для поиска в sql
        public static string poisk { get; set; }

        ///
        /// Для списка ингредиентов
        ///

        public static ObservableCollection<Ingr_data> collect_ing = new ObservableCollection<Ingr_data>();
        public static void Set_list_ingr()
        {
            SqlCommand s1 = new SqlCommand(@"Select Kod, Nazv from Ingredient where Nazv like N'"+poisk+"%'", conK);
            SqlDataReader read = s1.ExecuteReader();
            while (read.Read())
            {
                string n;
                int k;
                k = read.GetInt32(0);
                n = Convert.ToString(read.GetString(1));
                collect_ing.Add(new Ingr_data { kod_i = k.ToString(), nazv_i = n });
            }
            read.Close();
        }
        public static void Clear_list_ingr()
        {
            collect_ing.Clear();
        }
        public class Ingr_data
        {
            public string kod_i { get; set; }
            public string nazv_i { get; set; }
        }

     /// <summary>
     /// Для промежуточного хранения выбранного кода ингредиента
     /// </summary>
        private static string kod_i;
        public static void Set_kod_i(string k)
        {
            kod_i = k;
        }
        public static string Get_kod_i()
        {
            return kod_i;
        }
        public static void Clear_kod_i()
        {
            kod_i = "";
        }

     ///<summary>
     ///Для хранения списка разновидностей
     ///</summary>
        public static List<string> razn_k = new List<string>();
        private static ObservableCollection<string> razn_n = new ObservableCollection<string>();
        public static ObservableCollection<string> Razn_n
        {
            get
            {
                return razn_n;
            }
        }
        public static void Set_razn()
         {
             SqlCommand set1 = new SqlCommand(@"Select Kod, Nazv from Raznovidnosti_ingr where Kod_ingr = " + Get_kod_i(), conK);
            SqlDataReader dr1 = set1.ExecuteReader();
             while (dr1.Read())
             {
                 int k; string n;
                 k = dr1.GetInt32(0);
                 n = dr1.GetString(1);
                 razn_k.Add(k.ToString()); 
                 razn_n.Add(n);
             }
             dr1.Close();
        }
        public static void Delete_razn(string item)
        {
            int c = razn_k.Count();
            string kod_r = "0";
            if (c != 0)
            {
                for (int i = 1; i <= c; i++)
                {
                    if (Convert.ToInt32(item) == i - 1)
                    {
                        kod_r = razn_k[i - 1];
                        SqlCommand com = new SqlCommand(@"Delete from Raznovidnosti_ingr where kod_ingr = " + kod_i + "and kod = " + kod_r, conK);
                        com.ExecuteNonQuery();
                        Clear_razn();
                        Set_razn();
                    }
                }
            }
            else
            {

            }
        }
        public static void Clear_razn()
        {
            razn_k.Clear();
            razn_n.Clear();
        }
        //Нормальное хранение разновидностей ингредиента
        public static ObservableCollection<Raznovidn_tov> raz_ingr = new ObservableCollection<Raznovidn_tov>();
        public static void Set_raz_ingr(string a)
        {
            if (raz_ingr.Count!=0)
            {
                raz_ingr.Clear();
            }
            SqlCommand s1 = new SqlCommand("Select Ri.Kod, Ri.Nazv from Ingredient I right join Raznovidnosti_ingr Ri on I.Kod = Ri.Kod_ingr where I.Kod =" + a, conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                raz_ingr.Add(new Raznovidn_tov { kod = dr.GetInt32(0), nazv = dr.GetString(1) });
            }
            dr.Close();
            if (raz_ingr.Count() == 0)
            {
                raz_ingr.Add(new Raznovidn_tov { kod = 0, nazv = "нет" });
            }
        }
        public static Edinitci Set_edin_ingr(int k)
        {
            SqlCommand s1 = new SqlCommand("Select ed from Ingredient where Kod = " + k, conK);
            int index = (int)s1.ExecuteScalar();
            Edinitci ed = new Edinitci();
            foreach(var r in edin)
            {
                if (r.kod_ed == index)
                {
                    ed = r;
                }
            }
            return ed;
        }
        public static double Set_cena_ingr(int k)
        {
            SqlCommand s1 = new SqlCommand("Select Round(Cena,2) from Ingredient where kod = " + k, conK);
            double fl = 0;
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                fl = dr.GetDouble(0);
            }
            dr.Close();
            return fl;
        }

        /// <summary>
        /// Для промежуточного хранения выбранного кода товара
        /// </summary>
        
        public static int kod_t { get; set; }

        ///<summary>
        ///Для промежуточного хранения списка товаров
        ///</summary>
        public static ObservableCollection<Tovari> tovaris = new ObservableCollection<Tovari>();
        public static void Set_tovar()
        {
            SqlCommand set1 = new SqlCommand(@"Select T.Kod, T.Nazv, ed.Nazv from Tovari T, ed where T.ed = ed.Kod and Ingr = " + Get_kod_i(), conK);
            SqlDataReader dr1 = set1.ExecuteReader();

            while (dr1.Read())
            {
                int k; string n; string ed;
                k = dr1.GetInt32(0);
                n = dr1.GetString(1);
                ed = dr1.GetString(2);
                tovaris.Add(new Tovari { kod_t = k.ToString(), Nazv_t = n, ed_t = ed });
            }
            dr1.Close();
        }
        public static void Clear_tovari()
        {
            tovaris.Clear();
        }

     /// <summary>
     /// Список единиц
     /// </summary>
        public static List<Edinitci> edin = new List<Edinitci>();

        public static void Set_edin()
        {
            SqlCommand s16 = new SqlCommand(@"select Kod, Nazv from ed", conK);
            SqlDataReader sdr = s16.ExecuteReader();
            while (sdr.Read())
            {
                int k; string n;
                k = sdr.GetInt32(0);
                n = sdr.GetString(1);
                edin.Add(new Edinitci { kod_ed = k, nazv_ed = n });
            }
            sdr.Close();
        }
        public static void Clear_edin()
        {
            edin.Clear();
        }

        public static int kod_ed_ingr { get; set; }


        /// <summary>
        /// Список поставок
        /// </summary>

        public static ObservableCollection<Postavki> postavkis = new ObservableCollection<Postavki>();
        public static void Set_postav()
        {
            SqlCommand s1 = new SqlCommand("Select Pos.Kod, Pos.Data_post, P.Naim, W.Privet "
                + " from Postavka Pos, Postavshiki P, Worker W" 
                + " Where Pos.Postav = P.Kod and Pos.Sotr = W.Kod" 
                + " order by Pos.Kod desc", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                int k; string p, s; DateTime dt = new DateTime();
                k = dr.GetInt32(0);
                dt = dr.GetDateTime(1).Date;
                p = dr.GetString(2);
                s = dr.GetString(3);
                postavkis.Add(new Postavki { kod = k, data = dt, postav = p, sotr = s });
            }
            dr.Close();
        }
        public static void Clear_postav()
        {
            postavkis.Clear();
        }

        /// <summary>
        /// Добавление товаров в заказ
        /// </summary>
        
        public static int kod_post { get; set; }//Временное хранение кода поставки

        public static ObservableCollection<Post_zakaz> postzakazis = new ObservableCollection<Post_zakaz>();

        public static double sum_p;
        public static double Sum_ps
        {
            get
            {
                return sum_p;
            }
            set
            {
                sum_p = value;
                NotifyStaticPropertyChanged("Sum_ps");
            }
        }
        public static void Set_tovpost()
        {
            SqlCommand s1 = new SqlCommand("Select T.Kod, T.Nazv, [dbo].[Null_val](Ri.Kod), [dbo].[Null_str](Ri.Nazv), Sp.Kol, Sp.Cena_tov, ed.Nazv from Tovari T, Raznovidnosti_ingr Ri right join Sostav_postavki Sp on Ri.Kod = Sp.Razn, ed, Ingredient I where T.Kod = Sp.Tovar and T.Ingr = I.Kod and I.ed = ed.Kod and Sp.Kod_postavki = " + Util.kod_post, conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                int a = dr.GetInt32(0);
                string b = dr.GetString(1);
                int c = dr.GetInt32(2);
                string d = dr.GetString(3);
                double e = dr.GetDouble(4);
                double f = dr.GetDouble(5);
                string g = dr.GetString(6);
                postzakazis.Add(new Post_zakaz(a, b, c, d, e, f, g));
            }
            dr.Close();
            Sum_ps = Sum_post();
        }

        public static double Sum_post()
        {
            double sum = 0;
            foreach (var r in postzakazis)
            {
                sum += r.Obsh;
            }
            return Math.Round(sum, 2);
        }
        public static void Clear_tovpost()
        {
            postzakazis.Clear();
        }


        /// <summary>
        /// Изменение и добавление поставщика в поставке
        /// </summary>

        //Представление данных
        //Список поставщиков
        public static ObservableCollection<Add_Postavshiki> add_post = new ObservableCollection<Add_Postavshiki>();
        public static void Set_postav_add()
        {
            SqlCommand s1 = new SqlCommand("Select Kod, Naim from Postavshiki where Naim like N'" + poisk + "%'", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                add_post.Add(new Add_Postavshiki { kod = dr.GetInt32(0), nazv = dr.GetString(1) });
            }
            dr.Close();
        }
        public static void Clear_postav_add()
        {
            add_post.Clear();
        }

        public static ObservableCollection<Add_Sotr> add_sotr = new ObservableCollection<Add_Sotr>();
        public static void Set_sotr_add()
        {
            SqlCommand s1 = new SqlCommand("Select Kod, RTRIM(Familia)+N' '+RTRIM(Name)+N' '+RTRIM(Otch) as FIO from Worker where RTRIM(Familia)+N' '+RTRIM(Name)+N' '+RTRIM(Otch) like N'%"+poisk+"%'", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                add_sotr.Add(new Add_Sotr { kod = dr.GetInt32(0), fio = dr.GetString(1) });
            }
            dr.Close();

        }
        public static void Clear_sotr_add()
        {
            add_sotr.Clear();
        }

        //Список товаров в таблице поставки
        public static ObservableCollection<Tovari> tovarisadd = new ObservableCollection<Tovari>();
        public static void Set_tovar_post()
        {
            SqlCommand s1 = new SqlCommand("Select T.Kod, T.Nazv, ed.Nazv from Tovari T, ed where T.ed = ed.Kod and T.Nazv like N'"+poisk+ "%'", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                int a = dr.GetInt32(0);
                tovarisadd.Add(new Tovari { kod_t = a.ToString(), Nazv_t = dr.GetString(1), ed_t = dr.GetString(2) });
            }
            dr.Close();
        }
        public static void Clear_tovar_post()
        {
            tovarisadd.Clear();
        }
        public static ObservableCollection<Raznovidn_tov> raz_tov_add = new ObservableCollection<Raznovidn_tov>();
        public static void Set_raz_tov_add(string a)
        {
            SqlCommand s1 = new SqlCommand("Select Ri.Kod, Ri.Nazv from Tovari T, Ingredient I, Raznovidnosti_ingr Ri where T.Ingr = I.Kod and Ri.Kod_ingr = I.Kod and T.Kod = "+a, conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                raz_tov_add.Add(new Raznovidn_tov { kod = dr.GetInt32(0), nazv = dr.GetString(1) });
            }
            dr.Close();
            if (raz_tov_add.Count() == 0)
            {
                raz_tov_add.Add(new Raznovidn_tov { kod = 0, nazv = "нет" });
            }
        }
        public static void Clear_raz_tov_add()
        {
            raz_tov_add.Clear();
        }
        //Хранение выбранных данных
        //Поствщика
        private static int kod_set_p = -1;
        public static int Kod_set_p
        {
            get
            {
                return kod_set_p;
            }
            set
            {
                kod_set_p = value;
                NotifyStaticPropertyChanged("Kod_set_p");
            }
        }
        private static string naim_set_p;
        public static string Naim_set_p
        {
            get
            {
                return naim_set_p;
            }
            set
            {
                naim_set_p = value;
                NotifyStaticPropertyChanged("Naim_set_p");
            }
        }
        //Сотрудника
        private static int kod_set_s = -1;
        public static int Kod_set_s
        {
            get
            {
                return kod_set_s;
            }
            set
            {
                kod_set_s = value;
                NotifyStaticPropertyChanged("Kod_set_s");
            }
        }
        private static string naim_set_s;
        public static string Naim_set_s
        {
            get
            {
                return naim_set_s;
            }
            set
            {
                naim_set_s = value;
                NotifyStaticPropertyChanged("Naim_set_s");
            }
        }
        //Дата
        public static DateTime dt;
        public static DateTime Dt
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
                NotifyStaticPropertyChanged("DT");
            }
        }

        public static void Clear_data_postav()
        {
            Kod_set_p = -1;
            Kod_set_s = -1;
            Naim_set_p = "";
            Naim_set_s = "";
            kod_post = -1;
            
        }
        //Данные внутри формы Add_tov_post
        //единица измерения выбранного товара
        public static string ed_t { get; set; }
        public static void Set_ed_t(int kod_t)
        {
            SqlCommand s1 = new SqlCommand("Select ed.Nazv from Tovari T, ed where T.ed = ed.Kod and T.Kod =" + kod_t, conK);
            ed_t = s1.ExecuteScalar().ToString();
        }

        //Сохранение в БД
        public static bool Add_full_post()
        {
            //Проверка на ввод, пустые поля и формат ввода (уже на форме)
            try
            {
                SqlCommand s1 = new SqlCommand("insert into Postavka (Postav, Sotr, Data_post) values (" + Kod_set_p + "," + Kod_set_s + ", CONVERT(date, @dt)); SET @id=SCOPE_IDENTITY()", conK);
                SqlParameter p1 = new SqlParameter("dt", Dt);
                s1.Parameters.Add(p1);
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                s1.Parameters.Add(idParam);
                s1.ExecuteNonQuery();
                SqlCommand s2 = new SqlCommand("select top 1 Kod from Postavka order by Kod desc");
                int kod_p = (int)idParam.Value;
                int c = postzakazis.Count();
                for (int i = 0; i < c; i++)
                {
                    string p;
                    if (postzakazis[i].Kod_r == 0)
                    {
                        p = "null";
                    }
                    else
                    {
                        p = postzakazis[i].Kod_r.ToString();
                    }
                    SqlCommand s3 = new SqlCommand("insert into Sostav_postavki (Kod_postavki, Tovar, Cena_tov, Razn, kol)" +
                        "values (" + kod_p + "," + postzakazis[i].Kod_t + "," + postzakazis[i].Cena_ed + "," + p + "," + postzakazis[i].Kol + ")", conK);
                    s3.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



        //Данные поставщика

        
        public static Postavshik_info p = new Postavshik_info();
        
        public static void Set_post_info()
        {
            SqlCommand s1 = new SqlCommand("Select Naim, Tel, Address, Bik, Inn, Manager from Postavshiki where Kod = " + p.Kod.ToString(), conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                Util.p.Naim = dr.GetString(0);
                Util.p.Phone = dr.GetString(1);
                Util.p.Address = dr.GetString(2);
                Util.p.Bik = dr.GetString(3);
                Util.p.Inn = dr.GetString(4);
                Util.p.Manager = dr.GetString(5);
            }
            dr.Close();
        }

        public static bool Save_post_info()
        {
            try
            {
                SqlCommand s1 = new SqlCommand("insert into Postavshiki (Naim, Tel, Address, Bik, Inn, Manager) values ( N'" + p.Naim + "', N'" + p.Phone
                    + "' , N'" + p.Address + "', N'" + p.Bik + "', N'" + p.Inn + "', N'" + p.Manager + "')", conK);
                s1.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Update_post_info()
        {
            try
            {
                SqlCommand s1 = new SqlCommand("update Postavshiki set Naim = N'" + p.Naim + "' , Tel = N'" + p.Phone + "', Address = N'" + p.Address + "' , Bik = N'" + p.Bik
                    + "', Inn = N'" + p.Inn + "', Manager = N'" + p.Manager + "' where Kod = " + p.Kod, conK);
                s1.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete_post_info(int k)
        {
            try
            {
                SqlCommand s1 = new SqlCommand("delete from Postavshiki where kod = " + k, conK);
                s1.ExecuteNonQuery();
                Clear_postav_add();
                Set_postav_add();
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Данные склада
        public static ObservableCollection<Sklad_c> skladis = new ObservableCollection<Sklad_c>();

        public static void Set_sklad()
        {
            SqlCommand s1 = new SqlCommand("Select * from Sklad where Ingr like N'"+poisk+"%'", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                skladis.Add(new Sklad_c { Nazv_i = dr.GetString(0), Nazv_r = dr.GetString(1), Kol = dr.GetDouble(2), Nazv_ed = dr.GetString(3)});
            }
            dr.Close();
        }
        public static void Clear_sklad()
        {
            skladis.Clear();
        }
        //Данные списаний
        public static ObservableCollection<Prichina_c> prichina_oc = new ObservableCollection<Prichina_c>();
        public static void Set_prich_sp()
        {
            SqlCommand s1 = new SqlCommand("Select Kod, RTRIM(Naim) from Prichina", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                prichina_oc.Add(new Prichina_c { kod = dr.GetInt32(0), nazv = dr.GetString(1) });
            }
            dr.Close();
        }

        public static ObservableCollection<Spisanie_c> sp = new ObservableCollection<Spisanie_c>();
        public static void Set_sp()
        {
            SqlCommand s1 = new SqlCommand("select S.Kod, S.Date_sp, S.Kod_tovara, T.Nazv, dbo.Null_val(S.Razn), dbo.Null_str(R.Nazv), S.Kol, S.prichina, P.Naim, S.Sotr, RTRIM(W.Name)+' '+RTRIM(W.Familia), RTRIM(S.Comment), e.Nazv from Raznovidnosti_ingr R Right join Spisanie S on R.Kod = S.Razn left join Tovari T on S.Kod_tovara = T.Kod Left join Prichina P on S.prichina = P.Kod Left join Worker W on S.Sotr = W.Kod Left join ed e on T.Ingr in (Select Kod from Ingredient where ed = e.Kod)", conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                sp.Add(new Spisanie_c { Kod = dr.GetInt32(0), Data = dr.GetDateTime(1), Pz = new Post_zakaz {Kod_t = dr.GetInt32(2), 
                Nazv_t = dr.GetString(3), Kod_r = dr.GetInt32(4), Nazv_r = dr.GetString(5), Nazv_ed=dr.GetString(12)}, Kol = dr.GetFloat(6), Prich = new Prichina_c { kod = dr.GetInt32(7), nazv = dr.GetString(8) }, Sotr = new Add_Sotr{ kod = dr.GetInt32(9), fio = dr.GetString(10) }, Comment=dr.GetString(11)});
            }
            dr.Close();
        }
        public static void Clear_sp()
        {
            sp.Clear();
        }
            //Хранение выбранного индекса записи
            public static int index_sp;
            //Переменная для промежуточного хранения нового списания
            public static Spisanie_c sp_add = new Spisanie_c();
            public static void Initialize_sp_add()
        {
            sp_add.Pz = new Post_zakaz();
            sp_add.Sotr = new Add_Sotr();
            sp_add.Prich = new Prichina_c();
        }
            
            public static Spisanie_c Sp_add
            {
                get
                {
                return sp_add;
                }
            set
            {
                sp_add = value; NotifyStaticPropertyChanged("Sp_add");  
            }
            }
        //Сохранение списания
        public static bool Save_sp()
        {
            string s;
            if (Sp_add.Pz.Kod_r == 0)
            {
                s = "null";
            }
            else
            {
                s = Sp_add.Pz.Kod_r.ToString();
            }
            SqlCommand s1 = new SqlCommand("insert into Spisanie(Kod_tovara, Kol, prichina, Razn, Comment, Sotr, Date_sp)"
                +" values ("+Sp_add.Pz.Kod_t+","+Sp_add.Kol.ToString()+","+Sp_add.Prich.kod+","+ s +",N'"+Sp_add.Comment+"',"
                +Sp_add.Sotr.kod+",'"+Sp_add.Data+"')", conK);
           try
            {
                s1.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Получение единицы для товара по ингредиенту
        public static string Et_t_ingr(int kt)
        {
            SqlCommand s1 = new SqlCommand("Select ed.Nazv from Tovari T inner join Ingredient I on T.Ingr = I.Kod Inner join ed on I.ed = ed.Kod where T.Kod ="+kt, conK);
            string a;
            a = s1.ExecuteScalar().ToString();
            return a;
        }

        //Иерархия меню
        public static ObservableCollection<Tree> trees;
        private static ObservableCollection<Tree> Child(int kod_g, string nazv_g)
        {
            ObservableCollection<Tree> t = new ObservableCollection<Tree>();

            if (kod_g == -1)
            {
                t.Add(new Tree { G_g = new Group { Kod = kod_g, Nazv = nazv_g } });
                return t;
            }
            else
            {

                List<Group> g = new List<Group>();

                SqlCommand s1 = new SqlCommand("Select Kod, Nazv from Groups where Kod in (Select Kod_Group from Tree where Kod_Rod = " + kod_g + ") and Kod <> 1", conK);
                SqlDataReader dr1 = s1.ExecuteReader();
                while (dr1.Read())
                {
                    g.Add(new Group { Kod = dr1.GetInt32(0), Nazv = dr1.GetString(1) });
                }
                dr1.Close();

                if (g.Count <= 0)
                {
                }
                else
                {
                    foreach (var i in g)
                    {
                        SqlCommand s2 = new SqlCommand("Select Kod, Nazv from Menu where Kod in (Select Kod_Menu from Tree where Kod_Rod = " + i.Kod + " and Del = 0)", conK);
                        dr1 = s2.ExecuteReader();
                        ObservableCollection<Menu_c> m = new ObservableCollection<Menu_c>();
                        while (dr1.Read())
                        {
                            m.Add(new Menu_c { Kod = dr1.GetInt32(0), Nazv = dr1.GetString(1) });
                        }
                        dr1.Close();
                        t.Add(new Tree { G_g = new Group { Kod = i.Kod, Nazv = i.Nazv }, menu = new ObservableCollection<Menu_c>(m), c_g = new ObservableCollection<Tree>(Child(i.Kod, i.Nazv)) });
                    }
                }
                return t;
            }
        }
        public static void Set_trees()
        {
            trees = new ObservableCollection<Tree>();
            trees = Child(1, "Главная");
        }
        
        //Выбранное блюдо
        public static Select_menu sm;

        public static void Declare_sm()
        {
            sm = new Select_menu();
            sm.Ed = new Edinitci();
            sm.Menu = new Menu_c();
            sm.sostav = new ObservableCollection<Sostav_menu_c>();
        }
        public static void Set_bludo()
        {
            SqlCommand s1 = new SqlCommand("Select Koef_cen, ed from Menu where kod = " + sm.Menu.Kod, conK);
            SqlDataReader dr = s1.ExecuteReader();
            while (dr.Read())
            {
                sm.Koef_cen = dr.GetFloat(0);

                foreach (Edinitci e in edin)
                {
                    if (e.kod_ed == dr.GetInt32(1))
                    {
                        sm.Ed = e;
                    }
                }
            }
            dr.Close();
            sm.sostav = new ObservableCollection<Sostav_menu_c>();
            SqlCommand s2 = new SqlCommand("Select S.Kod_ingr, I.Nazv, dbo.Null_val(S.Razn), dbo.Null_str(Ri.Nazv), S.Kol_brut, S.Kol_nett, I.ed, I.Cena, I.Standart_brut, I.Del from Sostav S inner join Ingredient I on S.Kod_ingr = I.Kod left join Raznovidnosti_ingr Ri on S.Razn = Ri.Kod left join ed on I.ed = ed.Kod where S.Kod_menu = " + sm.Menu.Kod, conK);
            dr = s2.ExecuteReader();
            while (dr.Read())
            {
                Edinitci ed = new Edinitci();
                foreach (Edinitci e in edin)
                {
                    if (e.kod_ed == dr.GetInt32(6))
                    {
                        ed = e;
                    }
                }
                sm.sostav.Add(new Sostav_menu_c { Kod_i = dr.GetInt32(0), Nazv_i = dr.GetString(1), Razn = new Raznovidn_tov { kod = dr.GetInt32(2), nazv = dr.GetString(3) }, Kol_brut = dr.GetDouble(4), Kol_net = dr.GetDouble(5), Ed = ed, Cena = dr.GetDouble(7), St_brt = dr.GetDouble(8), Del=dr.GetBoolean(9) });
            }
        }
        private static double sum_b;
        public static double Sum_b { get { return sum_b; } set { sum_b = value; NotifyStaticPropertyChanged("Sum_b"); } }
        private static double sum_n;
        public static double Sum_n { get { return sum_n; } set { sum_n = value; NotifyStaticPropertyChanged("Sum_n"); } }
        public static double Sum_bludo()
        {
            double sum = 0;
            foreach (var r in sm.sostav)
            {
                sum += r.Kol_brut * r.Cena;
            }
            return Math.Round(sum*sm.Koef_cen, 2);
        }

        public static double Sum_net()
        {
            double sum = 0;
            foreach (var r in sm.sostav)
            {
                sum += r.Kol_net * r.St_brt;
            }
            return Math.Round(sum, 2);
        }
        public static void Save_bludo(bool b, Select_menu sm, Group g)
        {
            //b - True - существующее, False - новое.
            //sm - блюдо на вход
            if (b)
            {
                SqlCommand s1 = new SqlCommand("Update Menu set Nazv = N'"+sm.Menu.Nazv+ "', Koef_cen = @koefcen, ed = " + sm.Ed.kod_ed+" where Kod = "+sm.Menu.Kod, conK);
                SqlParameter p1 = new SqlParameter("@koefcen", sm.Koef_cen);
                s1.Parameters.Add(p1);
                s1.ExecuteNonQuery();
                SqlCommand s2 = new SqlCommand("Delete from Sostav where Kod_menu ="+sm.Menu.Kod, conK);
                s2.ExecuteNonQuery();
                foreach (var r in sm.sostav)
                {
                    SqlCommand s3 = new SqlCommand("insert into Sostav(Kod_menu, Kod_ingr, Kol_nett, Razn, Kol_brut) values"
                    + " (" + sm.Menu.Kod + "," + r.Kod_i + ", @kolnet , dbo.null_n(" + r.Razn.kod + "), @kolbr)", conK);
                    p1 = new SqlParameter("@kolbr", r.Kol_brut);
                    SqlParameter p2 = new SqlParameter("@kolnet", r.Kol_net);
                    s3.Parameters.Add(p1);
                    s3.Parameters.Add(p2);
                    s3.ExecuteNonQuery();
                }
            }
            else
            {
                SqlCommand s1 = new SqlCommand("insert into Menu (Nazv, Koef_cen, ed) values (N'"+sm.Menu.Nazv+"',@koefcen,"+sm.Ed.kod_ed+")", conK);
                SqlParameter p1 = new SqlParameter("@koefcen", sm.Koef_cen);
                s1.Parameters.Add(p1);
                s1.ExecuteNonQuery();
                SqlCommand s2 = new SqlCommand("Select top 1 kod from Menu order by Kod desc", conK);
                sm.Menu.Kod = (int)s2.ExecuteScalar();
                SqlCommand s4 = new SqlCommand("insert into Tree (Kod_Rod, Kod_Menu) values (" + g.Kod + "," + sm.Menu.Kod + ")", conK);
                s4.ExecuteNonQuery();
                foreach (var r in sm.sostav)
                {
                    SqlCommand s3 = new SqlCommand("insert into Sostav(Kod_menu, Kod_ingr, Kol_nett, Razn, Kol_brut) values"
                    + " (" + sm.Menu.Kod + "," + r.Kod_i + ", @kolnet , dbo.null_n(" + r.Razn.kod + "), @kolbr)", conK);
                    p1 = new SqlParameter("@kolbr", r.Kol_brut);
                    SqlParameter p2 = new SqlParameter("@kolnet", r.Kol_net);
                    s3.Parameters.Add(p1);
                    s3.Parameters.Add(p2);
                    s3.ExecuteNonQuery();
                }
            }
        }
        public static void Delete_bludo(int kod)
        {
            SqlCommand s1 = new SqlCommand("Update Menu set Del = 1 where Kod = "+kod, conK);
            s1.ExecuteNonQuery();
        }

        public static void Delete_group()
        {
            SqlCommand s1 = new SqlCommand("");
        }

        //Реализация INPC для статических свойств в статическом классе
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (StaticPropertyChanged != null)
            {
                StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    //Классы, реализующие представление данных в главных вкладках
    public class Tovari
    {
        public string kod_t { get; set; }
        public string Nazv_t { get; set; }
        public string ed_t { get; set; }
    }
    public class Edinitci
    {
        public int kod_ed { get; set; }
        public string nazv_ed { get; set; }
    }

    public class Postavki
    {
        public int kod { get; set; }
        public DateTime data { get; set; }
        public string sotr { get; set; }
        public string postav { get; set; }
    }
    public class Sotrudnik 
    {
        public int kod { get; set; }
        public string fio { get; set; }
        public int spec { get; set; }
    }
    //Логика для отображения отдельной поставки
    public class Post_zakaz : INotifyPropertyChanged
    {
        public Post_zakaz()
        {

        }
        public Post_zakaz(int kod_t, string nazv_t, int kod_r, string nazv_r, double kol, double cena_ed, string nazv_ed)
        {
            this.kod_t = kod_t;
            this.nazv_t = nazv_t;
            this.kod_r = kod_r;
            this.nazv_r = nazv_r;
            this.kol = kol;
            this.cena_ed = cena_ed;
            this.obsh = kol * cena_ed;
            this.nazv_ed = nazv_ed;
        }

        public Post_zakaz(int kod_t, string nazv_t, int kod_r, string nazv_r, string nazv_ed)
        {
            this.kod_t = kod_t;
            this.nazv_t = nazv_t;
            this.kod_r = kod_r;
            this.nazv_r = nazv_r;
            this.nazv_ed = nazv_ed;
        }

        private int kod_t, kod_r;
        private string nazv_t, nazv_r, nazv_ed;
        public double kol, cena_ed, obsh;
        public int Kod_t 
        { get { return kod_t; } set { kod_t = value; OnPropertyChanged(); } }
        public string Nazv_t
        { get { return nazv_t; } set { nazv_t = value; OnPropertyChanged(); } }
        public int Kod_r
        { get { return kod_r; } set { kod_r = value; OnPropertyChanged(); } }
        public string Nazv_r
        { get { return nazv_r; } set { nazv_r = value; OnPropertyChanged(); } }
        public double Kol
        { get { return kol; } set { kol = value; Util.Sum_ps = Util.Sum_post(); OnPropertyChanged("Obsh"); } }
        public double Cena_ed
        { get { return cena_ed; } set { cena_ed = value; Util.Sum_ps = Util.Sum_post(); OnPropertyChanged("Obsh"); } }
        public double Obsh
        { get { return kol * cena_ed; } }
        public string Nazv_ed
        {
            get
            {
                return nazv_ed;
            }
            set
            {
                nazv_ed = value;
                OnPropertyChanged("Nazv_ed");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Add_Postavshiki
    {
        public int kod { get; set; }
        public string nazv { get; set; }
    }
    public class Add_Sotr : INotifyPropertyChanged
    {
        private int Kod;
        private string Fio;
        public int kod { get { return Kod; } set { Kod = value; OnPropertyChanged("kod"); } }
        public string fio { get { return Fio; } set { Fio = value; OnPropertyChanged("fio"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class Raznovidn_tov
    {
        public int kod { get; set; }
        public string nazv { get; set; }
    }

    public class Postavshik_info : INotifyPropertyChanged
    {
        int kod;
        string naim;
        string phone;
        string address;
        string bik;
        string inn;
        string manager;

        public int Kod
        {
            get
            {
                return kod;
            }
            set
            {
                kod = value; OnPropertyChanged("Kod");
            }
        }

        public string Naim
        {
            get
            {
                return naim;
            }
            set
            {
                naim = value; OnPropertyChanged("Naim");
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value; OnPropertyChanged("Phone");
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value; OnPropertyChanged("Address");
            }
        }

        public string Bik
        {
            get
            {
                return bik;
            }
            set
            {
                bik = value; OnPropertyChanged("Bik");
            }
        }

        public string Inn
        {
            get
            {
                return inn;
            }
            set
            {
                inn = value; OnPropertyChanged("Inn");
            }
        }

        public string Manager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value; OnPropertyChanged("Manager");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Sklad_c
    {
        public string Nazv_i { get; set; }
        public string Nazv_r { get; set; }
        public double Kol { get; set; }
        public string Nazv_ed { get; set; }
    }

    //Классы для списания
    public class Spisanie_c : INotifyPropertyChanged
    {
        int kod;
        DateTime data;
        Post_zakaz pz;
        double kol;
        string comment;
        Add_Sotr sotr;
        Prichina_c prich;

        public int Kod
        {
            get
            {
                return kod;
            }
            set
            {
                kod = value; OnPropertyChanged("Kod");
            }
        }
        public DateTime Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value; OnPropertyChanged("Data");
            }
        }
        public Post_zakaz Pz
        {
            get
            {
                return pz;
            }
            set
            {
                pz = value; OnPropertyChanged("Pz");
            }
        }
        public double Kol
        {
            get
            {
                return kol;
            }
            set
            {
                kol = value; OnPropertyChanged("Kol");
            }
        }
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value; OnPropertyChanged("Comment");
            }
        }
        public Add_Sotr Sotr
        {
            get
            {
                return sotr;
            }
            set
            {
                sotr = value; OnPropertyChanged("Sotr");
            }
        }
        public Prichina_c Prich
        {
            get
            {
                return prich;
            }
            set
            {
                prich = value; OnPropertyChanged("Prich");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Prichina_c
    {
        public int kod { get; set; }
        public string nazv { get; set; }
    }

    //классы для представления меню блюд
    public class Menu_c : INotifyPropertyChanged
    {
        private int kod;
        private string nazv;
        public int Kod { get { return kod; } set { kod = value; } }
        public string Nazv { get { return nazv; } set { nazv = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class Sostav_menu_c : INotifyPropertyChanged
    {
        private int kod_i;
        private string nazv_i;
        private Edinitci ed;
        private double kol_brut;
        private double kol_net;
        private Raznovidn_tov razn;
        private double cena;
        private double st_brt;
        private bool del;

        public int Kod_i { get { return kod_i; } set { kod_i = value; OnPropertyChanged("Kod_i"); } }
        public string Nazv_i { get { return nazv_i; } set { nazv_i = value; OnPropertyChanged("Nazv_i"); } }
        //Условие на целое число
        public double Kol_brut { get { return kol_brut; } set { kol_brut = value; Util.Sum_b = Util.Sum_bludo(); OnPropertyChanged("Kol_brut"); } }
        public double Kol_net { get { return kol_net; } set { kol_net = value>kol_brut ? kol_brut:value ; Util.Sum_n = Util.Sum_net(); OnPropertyChanged("Kol_net"); } }
        public Raznovidn_tov Razn { get { return razn; } set { razn = value; OnPropertyChanged("Razn"); } }
        public Edinitci Ed { get { return ed; } set { ed = value; OnPropertyChanged("Ed"); } }
        public double Cena { get { return cena; } set { cena = value; OnPropertyChanged("Cena"); } }
        public double St_brt { get { return st_brt; } set { st_brt = value; OnPropertyChanged("St_brt"); } }
        public bool Del { get { return del; } set { del = value; OnPropertyChanged("Del"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
    public class Group
    {
        private int kod;
        private string nazv;
        public int Kod { get { return kod; } set { kod = value; } }
        public string Nazv { get { return nazv; } set { nazv = value; } }

    }
    public class Tree
    {
        private Group g_g;
        public ObservableCollection<Tree> c_g { get; set; }
        public ObservableCollection<Menu_c> menu { get; set; }
        public Group G_g { get { return g_g; } set { g_g = value; } }
    }
    public class Select_menu : INotifyPropertyChanged
    {
        private Menu_c menu;
        private float koef_cen;
        private Edinitci ed;
        public float Koef_cen { get { return koef_cen; } set { koef_cen = value<1?1:value; Util.Sum_b = Util.Sum_bludo(); OnPropertyChanged("Koef_cen"); } }
        public Edinitci Ed { get { return ed; } set { ed = value; OnPropertyChanged("Ed"); } }
        public Menu_c Menu { get { return menu; } set { menu = value; OnPropertyChanged("Menu"); } }
        public ObservableCollection<Sostav_menu_c> sostav { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

