using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Libro : Articolo
    {
        public int NumPagine { get; set; }

        override public string CalcolaCodice(int index)
        {
            string codice = index.ToString().PadLeft(4, '0');
            return codice;
        }
        public Libro(int codiceArticolo, string titolo, int anno, string autore, genere genere,string foto, int numPagine)
        {
            base.CodiceArticolo = CalcolaCodice(codiceArticolo);
            base.Titolo = titolo;
            base.Anno = anno;
            base.Autori = RicavaAutori(autore);
            //base.Scaffale = scaffale;
            base.Genere = genere;
            base.Foto = RicavaFoto(foto);
            this.NumPagine = numPagine;
        }

    }
}