using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Autore
    {
        public string NomeAutore { get; set; }
        public string CognomeAutore { get; set; }
        public Autore(string nome, string cognome)
        {
            this.NomeAutore = nome;
            this.CognomeAutore = cognome;
        }
    }
}