using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using Word = Microsoft.Office.Interop.Word;

namespace TestKafe
{
    class Word_otch
    {
        private object Template;
        private Word.Application WordApp;
        private Word.Document WordDoc;
        
        //Base method
        private void Set_template(object temp)
        {
            WordApp = new Word.Application();
            WordApp.Visible = false;
            WordDoc = WordApp.Documents.Open(temp);
        }
        private void Stub_replace(string zaglishka, string text, Word.Range range)
        {
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: zaglishka, ReplaceWith: text);
        }
        //
        public void Bludo(Select_menu menu)
        {
            Template = System.IO.Path.GetFullPath(@"Template_otch\Bludo.dot");
            Set_template(Template);
            var Shapka = WordDoc.Bookmarks["range1"].Range;
            Stub_replace("{nazv}", menu.Menu.Nazv, Shapka);
            Shapka = WordDoc.Bookmarks["range1"].Range;
            Stub_replace("{nacen}", menu.Koef_cen.ToString(), Shapka);
            Shapka = WordDoc.Bookmarks["range1"].Range;
            Stub_replace("{ed}", menu.Ed.nazv_ed, Shapka);
            Shapka = WordDoc.Bookmarks["range1"].Range;
            Stub_replace("{vesbl}", Util.Sum_n.ToString(), Shapka);
            Shapka = WordDoc.Bookmarks["range1"].Range;
            Stub_replace("{cena}", Util.Sum_b.ToString(), Shapka);
            Word.Table table = WordDoc.Tables[1];
            int count = menu.sostav.Count();
            for (int i=0; i<count; i++)
            {
                if (i == 0)
                {

                }
                else
                {
                    WordDoc.Tables[1].Rows.Add();
                }
                string razn = "";
                if (menu.sostav[i].Razn.kod == 0)
                {
                    razn = "";
                }
                else
                {
                    razn = "("+menu.sostav[i].Razn.nazv+")";
                }
                WordDoc.Tables[1].Cell(i + 2, 1).Range.Text = menu.sostav[i].Nazv_i.Trim() +razn;
                WordDoc.Tables[1].Cell(i + 2, 2).Range.Text = menu.sostav[i].Ed.nazv_ed.Trim();
                WordDoc.Tables[1].Cell(i + 2, 3).Range.Text = menu.sostav[i].Kol_brut.ToString();
                WordDoc.Tables[1].Cell(i + 2, 4).Range.Text = menu.sostav[i].Kol_net.ToString();
            }
            WordDoc.SaveAs(Util.Get_otch());
            WordApp.Visible = true;
        }

    }
}
