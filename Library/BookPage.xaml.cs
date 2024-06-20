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
using static System.Reflection.Metadata.BlobBuilder;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для BookPage.xaml
    /// </summary>
    
    public partial class BookPage : Page
    {
        private readonly DataBaseService dbHelper = AppServices.DbHelper;
        public Book Book { get; set; }
        public BookPage(Book book)
        {
            InitializeComponent();
            Book = book;
            List<Book> similarBooks = dbHelper.FindSimilarBooks(Book);
            BooksListBox.ItemsSource = similarBooks;
            this.DataContext = this;
        }

        private void BooksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BooksListBox.SelectedItem is Book selectedBook)
            {
                BookPage bookPage = new BookPage(selectedBook);
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToPage(bookPage);
                }
            }
        }
    }
}
