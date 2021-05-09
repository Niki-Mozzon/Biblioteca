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
using System.Media;

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for FormUtente.xaml
    /// </summary>
    public partial class FormUtente : Window
    {
        public static Library biblioteca = new Library();
        static int random = 0;
        int sizeArray = 0;
        int sizePrestitiUtente = 0;
        int sizePrestitiBiblioteca = 0;
        //Utente user = FileManager.user;
        Utente user = new Utente("Tizio", "De Tizi", "tizio.detizi@tizis.com", "12125125", "tizio123");


        public FormUtente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            biblioteca.Users.Append(user);
            CaricaDati();
            Pulisci();
            listPrestati.Items.Clear();
            listDvd.Items.Clear();
            for (int i = 0; i < biblioteca.Articoli.Length; i++)
            {
                RiempiListe(biblioteca.Articoli[i]);
            }
            Random rndm = new Random();
            random = rndm.Next(0, biblioteca.Articoli.Length);
            if (biblioteca.Articoli.Length != 0)
            {
                imgBtnRandom.ImageSource = GetUri(@biblioteca.Articoli[random]);
                imgBtnLast.ImageSource = GetUri(@biblioteca.Articoli[biblioteca.Articoli.Length - 1]);
                //btnSelectRandom.Content = biblioteca.Articoli[random].Titolo;
                //btnLast.Content = biblioteca.Articoli[biblioteca.Articoli.Length - 1].Titolo;
            }
            _lblWelcome.Content = $"Welcome back, {user.Nome}!";
            listAll.DisplayMemberPath = @"Titolo";
            listAll.SelectedValuePath = @"CodiceArticolo";
            listDvd.DisplayMemberPath = @"Titolo";
            listDvd.SelectedValuePath = @"CodiceArticolo";
            listBook.DisplayMemberPath = @"Titolo";
            listBook.SelectedValuePath = @"CodiceArticolo";
            listBoxInPrestito.DisplayMemberPath = @"Titolo";
            listBoxInPrestito.SelectedValuePath = @"CodiceArticolo";
            listPrestati.DisplayMemberPath = @"Titolo";
            listPrestati.SelectedValuePath = @"CodiceArticolo";
            imgCover.Stretch = Stretch.Uniform;
            imgBtnRandom.Stretch = Stretch.Uniform;
            imgBtnLast.Stretch = Stretch.Uniform;
            //imgLast.Source = GetUri(@biblioteca.Articoli[biblioteca.Articoli.Length - 1]);
        }

        //|||||||||||||||EVENTS|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        private void listAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            listBoxInPrestito.SelectedIndex = -1;
            listPrestati.SelectedIndex = -1;
            var itemSelected = listAll.SelectedItem;
            PrelevaSelezione(itemSelected);
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            Articolo item = GetItem() as Articolo;
            if (item != null)
            {
                if (item.Prestato == false)
                {
                    Array.Resize(ref user.PrestitiBiblioteca, user.PrestitiBiblioteca.Length + 1);
                    Array.Resize(ref biblioteca.PrestitiBiblioteca, biblioteca.PrestitiBiblioteca.Length + 1);
                    Prestito prestito = new Prestito(sizePrestitiBiblioteca, DateTime.Now, item, user);
                    user.PrestitiBiblioteca[sizePrestitiUtente++] = prestito;
                    biblioteca.PrestitiBiblioteca[sizePrestitiBiblioteca++] = prestito;
                    item.Prestato = true;
                    listBoxInPrestito.Items.Add(item);
                    listPrestati.Items.Add(item);
                    PrelevaSelezione(item);
                }
                else
                {
                    MessageBox.Show("The item is currently not available.");
                }
            }
            else
            {
                MessageBox.Show("Error: Not item selected");
            }
        }

        private void comboGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            listAll.Items.Clear();
            listDvd.Items.Clear();
            listBook.Items.Clear();
            //listBoxInPrestito.Items.Clear();
            //listPrestati.Items.Clear();
            ExpAutori.IsExpanded = false;
            for (int i = 0; i < biblioteca.Articoli.Length; i++)
            {
                if (comboGenre.SelectedIndex == 0)
                {
                    RiempiListe(biblioteca.Articoli[i]);
                }
                else if ((genere)Enum.Parse(typeof(genere), (comboGenre.SelectedItem).ToString()) == biblioteca.Articoli[i].Genere)
                {
                    RiempiListe(biblioteca.Articoli[i]);
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                Pulisci();
                listAll.SelectedIndex = -1;
                listDvd.SelectedIndex = -1;
                listBook.SelectedIndex = -1;
            }
        }

        private void listBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            var itemSelected = listBook.SelectedItem;
            PrelevaSelezione(itemSelected);
        }

        private void listDvd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            var itemSelected = listDvd.SelectedItem;
            PrelevaSelezione(itemSelected);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            comboGenre.SelectedIndex = 0;
            string search = txtSearch.Text.ToLower();
            object item = null;
            for (int i = 0; i < biblioteca.Articoli.Length; i++)
            {
                if (search == biblioteca.Articoli[i].CodiceArticolo.ToLower())
                {
                    item = biblioteca.Articoli[i];
                }
                else if (search == biblioteca.Articoli[i].Titolo.ToLower())
                {
                    item = biblioteca.Articoli[i];
                }
            }
            if (item == null)
            {
                MessageBox.Show("No result.");
            }
            else
            {
                PrelevaSelezione(item);
            }
            TabArticoli.SelectedIndex = 0;
            listAll.SelectedItem = item;

        }

        private void listBoxInPrestito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            comboGenre.SelectedIndex = 0;
            TabArticoli.SelectedIndex = 0;
            var itemSelected = listBoxInPrestito.SelectedItem;
            listAll.SelectedIndex = -1;
            listAll.SelectedItem = itemSelected;
            PrelevaSelezione(itemSelected);
        }

        private void listPrestati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pulisci();
            comboGenre.SelectedIndex = 0;
            TabArticoli.SelectedIndex = 0;
            var itemSelected = listPrestati.SelectedItem;
            listAll.SelectedIndex = -1;
            listAll.SelectedItem = itemSelected;
            PrelevaSelezione(itemSelected);
        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                Pulisci();
                listPrestati.SelectedIndex = -1;
                listBoxInPrestito.SelectedIndex = -1;
            }
        }



        //|||||||||||||||||METHODS|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        private void RiempiListe(Articolo item)
        {
            listAll.Items.Add(item);
            if (item is Video)
            {
                listDvd.Items.Add(item);
                if (item.Prestato == true)
                {
                    //listBoxInPrestito.Items.Add(item);
                    //listPrestati.Items.Add(item);
                }
            }
            if (item is Libro)
            {
                listBook.Items.Add(item);
                if (item.Prestato == true)
                {
                    //listBoxInPrestito.Items.Add(item);
                    //listPrestati.Items.Add(item);
                }
            }

        }

        private void CaricaDati()
        {
            string nameFile = FileManager.FileDocs;
            string pathDirectory = System.IO.Path.GetFullPath(FileManager.FileFolder);
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
                        string line = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();
                            if (!(line == ""))
                            {
                                Array.Resize(ref biblioteca.Articoli, (biblioteca.Articoli.Length + 1));
                                string[] items = line.Split(';');
                                if (items[0].ToLower() == "book")
                                {
                                    biblioteca.Articoli[sizeArray] = new Libro(sizeArray, items[1], Convert.ToInt32(items[2]), items[3], (genere)Enum.Parse(typeof(genere), items[4]), items[5], Convert.ToInt32(items[6]));
                                }
                                else if (items[0].ToLower() == "dvd")
                                {
                                    biblioteca.Articoli[sizeArray] = new Video(sizeArray, items[1], Convert.ToInt32(items[2]), items[3], ((genere)Enum.Parse(typeof(genere), items[4])), items[5], Convert.ToInt32(items[6]));
                                }
                                sizeArray++;
                            }
                        }
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }
        }

        private void PrelevaSelezione(object itemSelected)
        {
            if (itemSelected != null)
            {
                Articolo item = itemSelected as Articolo;
                Mostra(item);
            }
        }

        private void Pulisci()
        {
            listAutori.Items.Clear();
            lblCode.Content = "";
            lblTitle.Content = "";
            lblGenre.Content = "";
            lblYear.Content = "";
            lblAvailable.Content = "";
            ExpAutori.IsEnabled = false;
            lbLength.Content = "";
            lblAvailable.Content = "";
            lblTerm.Content = "";
            lblDate.Content = "";
            lblLendCode.Content = "";
            imgCover.Source = null;
        }

        private void Mostra(Articolo articolo)
        {
            ExpAutori.IsEnabled = true;
            lblCode.Content = "Code: " + articolo.CodiceArticolo;
            lblTitle.Content = "Title: " + articolo.Titolo;
            lblGenre.Content = "Genre: " + articolo.Genere;
            lblYear.Content = "Year: " + articolo.Anno;
            lblAvailable.Content = "Availability: " + articolo.Disponibilita();
            foreach (Autore item in articolo.Autori)
            {
                listAutori.Items.Add(item.NomeAutore + " " + item.CognomeAutore);
            }
            for (int i = 0; i < user.PrestitiBiblioteca.Length; i++)
            {
                if (articolo.CodiceArticolo == user.PrestitiBiblioteca[i].ArticoloPrestato.CodiceArticolo)
                {
                    lblTerm.Content = "Lending term: " + user.PrestitiBiblioteca[i].DataPrestito.AddMonths(1).ToString();
                    lblDate.Content = "Lending date: " + user.PrestitiBiblioteca[i].DataPrestito.ToString();
                    lblLendCode.Content = "Lend code: " + user.PrestitiBiblioteca[i].CodicePrestito;
                }
            }
            if (articolo is Video)
            {
                Video film = articolo as Video;
                lbLength.Content = "Running time: " + film.Durata;
            }
            else if (articolo is Libro)
            {
                Libro book = articolo as Libro;
                lbLength.Content = "Pages: " + book.NumPagine;
            }
            var immagine = GetUri(articolo);
            imgCover.Source = immagine;

        }

        private object GetItem()
        {
            object item;
            if (listAll.SelectedIndex != -1)
            {
                item = listAll.SelectedItem;
            }
            else if (listBook.SelectedIndex != -1)
            {
                item = listBook.SelectedItem;
            }
            else if (listDvd.SelectedIndex != -1)
            {
                item = listDvd.SelectedItem;
            }
            else
            {
                item = null;
            }
            return item;
        }

        private void btnUpdateRandom_Click(object sender, RoutedEventArgs e)
        {
            Random rndm = new Random();
            random = rndm.Next(0, biblioteca.Articoli.Length);
            if (biblioteca.Articoli.Length != 0)
            {
                //btnSelectRandom.Content = biblioteca.Articoli[random].Titolo;
                imgBtnRandom.ImageSource = GetUri(@biblioteca.Articoli[random]);
            }
        }

        private void btnSelectRandom_Click(object sender, RoutedEventArgs e)
        {
            if (biblioteca.Articoli.Length != 0)
            {
                Articolo item = biblioteca.Articoli[random];
                PrelevaSelezione(item);
                TabArticoli.SelectedIndex = 0;
                listAll.SelectedItem = item;
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            if (biblioteca.Articoli.Length != 0)
            {
                Articolo item = biblioteca.Articoli[biblioteca.Articoli.Length - 1];
                PrelevaSelezione(item);
                TabArticoli.SelectedIndex = 0;
                listAll.SelectedItem = item;
            }
        }

        private BitmapImage GetUri(Articolo item)
        {
            string path = item.Foto;
            Uri fileUri = new Uri(@path,UriKind.Relative);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.None;
            image.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = fileUri;
            image.EndInit();
            return image;
        }

    }
}