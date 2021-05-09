using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CaricaUtenti();
        }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Registrazione reg = new Registrazione();
            this.Close();
            reg.ShowDialog();
        }

        private void CaricaUtenti()
        {
            string nameFile = FileManager.FileUsers;
            string pathDirectory = System.IO.Path.GetFullPath("Streamer/");
            string pathComplete = System.IO.Path.Combine(pathDirectory + nameFile);
            if (!Directory.Exists(pathDirectory))
            {
                MessageBox.Show("Some files seem displaced.");
            }
            else
            {
                if (!File.Exists(pathComplete))
                {
                    MessageBox.Show("Some files seem missing.");
                }
                else
                {
                    using (StreamReader sr = new StreamReader(pathComplete))
                    {
                        while (!sr.EndOfStream)
                        {
                            var line = sr.ReadLine();
                            if (!(line == ""))
                            {
                                string[] items = line.Split(';');
                                FormUtente.biblioteca.Users.Add(new Utente(items[0], items[1], items[2], items[3], items[4]));
                            }
                        }
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            Utente user;
            bool accesso = false;
            foreach (Utente item in FormUtente.biblioteca.Users)
            {
                if (item.Mail == txtMail.Text)
                {
                    if (item.Password == txtPass.Password)
                    {
                        accesso = true;
                        user = item;
                        FileManager.user = user;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("The password seems not correct.");
                    }
                }
            }
                if (accesso)
                {
                    FormUtente fu = new FormUtente();
                    this.Close();
                    fu.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Error: Username or password are not correct.");
                }
        }
    }
}
