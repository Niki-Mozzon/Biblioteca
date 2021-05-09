using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Utente
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Tel { get; set; }
        public string Codice { get; set; }
        public Prestito[] PrestitiBiblioteca = new Prestito[0];

        public Utente(string nome, string cognome, string mail, string tel, string password)
        {
            this.Nome = nome;
            this.Cognome = cognome;
            this.Mail = mail;
            this.Password = password;
            this.Tel = tel;
        }
    }
}