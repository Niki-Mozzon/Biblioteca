using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{

    public class Library
    {
        public List<Utente> Users= new List<Utente>();
        public Articolo[] Articoli = new Articolo[0];
        public Prestito[] PrestitiBiblioteca = new Prestito[0];
    }
}