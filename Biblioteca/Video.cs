using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Video : Articolo
    {
        public int Durata { get; set; }

        public Video(int codiceArticolo, string titolo, int anno, string autore, genere genere, string foto, int durata)
        {
            base.CodiceArticolo = CalcolaCodice(codiceArticolo);
            base.Titolo = titolo;
            base.Anno = anno;
            base.Autori = RicavaAutori(autore);
            //base.Scaffale = scaffale;
            base.Genere = genere;
            base.Foto = RicavaFoto(foto);
            this.Durata = durata;
        }
    }
}