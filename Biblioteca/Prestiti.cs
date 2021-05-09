using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca
{
    public class Prestito
    {
        public string CodicePrestito { get; set; }

        public DateTime DataPrestito { get; set; }

        public Articolo ArticoloPrestato { get; set; }

        public Utente PrestatoAUtente { get; set; }

        virtual public string CalcolaCodice(int index)
        {
            string codice = index.ToString().PadLeft(4, '0');
            codice = this.PrestatoAUtente.Nome.Remove(2) + this.PrestatoAUtente.Cognome.Remove(2) + codice;
            return codice;
        }

        public Prestito(int index, DateTime dataPrestito, Articolo articoloPrestato, Utente PrestatoAUtente)
        {
            this.ArticoloPrestato = articoloPrestato;
            this.PrestatoAUtente = PrestatoAUtente;
            this.DataPrestito = dataPrestito;
            this.CodicePrestito = CalcolaCodice(index);
        }
    }
}