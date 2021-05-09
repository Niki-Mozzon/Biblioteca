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
using System.Windows.Shapes;
using System.IO;

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for Registrazione.xaml
    /// </summary>
    public partial class Registrazione : Window
    {
        public Registrazione()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mW = new MainWindow();
            this.Close();
            mW.ShowDialog();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bool taken = false;
            string[] x = GetVariables();
            if (x[4] != x[4])
            {
                MessageBox.Show("The password confirmed is not correct!");
                txtPassConfirm.Clear();
                txtPass.Clear();
            }
            else if (!(x[2].Contains('@') && x[2].Contains('.')))
            {
                MessageBox.Show("The e-mail address inserted is not valid!");
            }
            else if(x[0]==""|| x[1] == "" || x[2] == "" || x[3] == "" || x[4] == "")
            {
                MessageBox.Show("You cannot leave fields empty!");
            }
            else
            {
                foreach (Utente item in FormUtente.biblioteca.Users)
                {
                    if (item.Mail == txtMail.Text)
                    {
                        taken = true;
                        MessageBox.Show("The mail is already registered.");
                        break;

                    }
                }
                if (!taken)
                {
                    Utente user = new Utente(x[0], x[1], x[2], x[3], x[4]);
                    FormUtente.biblioteca.Users.Add(user);
                    AddUser(user);
                    FileManager.user = user;
                    //Send e mail to that email address
                    MessageBox.Show("A verification e-mail has been sent to your e-mail box.\nPlease open it to activate your account.. scherzo!");
                    FormUtente fu = new FormUtente();
                    this.Close();
                    fu.ShowDialog();
                }
            }
        }

        private string[] GetVariables()
        {
            string[] x = new string[6];
            string name = txtNome.Text;
            string lastName = txtCognome.Text;
            x[0] = char.ToUpper(name[0]) +name.Substring(1);
            x[1] = char.ToUpper(lastName[0]) + lastName.Substring(1);
            x[2] = txtMail.Text;
            x[3] = txtPhone.Text;
            x[4] = txtPass.Password; 
            x[5] = txtPassConfirm.Password;
            return x;
        }

        private void AddUser(Utente user)
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
                    using (StreamWriter sw = new StreamWriter(pathComplete, append: true))
                    {
                        sw.WriteLine($"{user.Nome};{user.Cognome};{user.Mail};{user.Tel};{user.Password};");
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
        }

    }
}
