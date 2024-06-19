using System.Net;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace Library
{
    public partial class MainWindow : Window
    {
        public Book testBook { get; set; }
        public Genre testGenre { get; set; }
        public Author testAuthor { get; set; }
        public Publisher testPublisher { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Host=192.168.100.5;Username=postgres;Password=root;Database=Library";
            DataBaseService dbHelper = new DataBaseService(connectionString);
            List<Book> books = dbHelper.GetAllBooks();
            BooksListBox.ItemsSource = books;

            DataContext = this;
        }
    }
}