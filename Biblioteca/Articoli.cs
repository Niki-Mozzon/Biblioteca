using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Articolo
    {
        public string CodiceArticolo { get; set; }
        public string Titolo { get; set; }
        public bool Prestato = false;
        public int Anno { get; set; }
        public string Scaffale { get; set; }
        public Autore[] Autori { get; set; }
        public string @Foto { get; set; }
        public genere Genere { get; set; }

        public string Disponibilita()
        {
            string answer;
            if (this.Prestato == false)
            {
                answer = "Available";
            }
            else
            {
                answer = "Borrowed";
            }
            return answer;
        }
        virtual public string CalcolaCodice(int index)
        {
            string codice = index.ToString().PadLeft(4, '0');
            codice = codice.Insert(2, "-");
            return codice;
        }
        public Autore[] RicavaAutori(string autore)
        {
            string[] nomi = autore.Split(',');
            Autore[] autori = new Autore[nomi.Length];
            for (int i = 0; i < autori.Length; i++)
            {
                string[] items = nomi[i].Split(' ');
                autori[i] = new Autore(items[0], items[items.Length - 1]);
            }
            return autori;
        }

        public string RicavaFoto(string nameFoto)
        {
            string path = System.IO.Path.GetFullPath(FileManager.Covers);
            string pathFile = System.IO.Path.Combine(path + nameFoto);
            return pathFile;
        }
    }
    public enum genere
    {
        Horror,
        Fantasy,
        Adventure,
        Romantic,
        Action,
        Comedy,
        Musical,
        Novella
    }
}