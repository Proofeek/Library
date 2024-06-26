using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        private readonly DataBaseService dbHelper = AppServices.DbHelper;
        private List<Book> returnedBooks = new List<Book>();
        private List<Book> unReturnedBooks = new List<Book>();
        private List<Book> unavailabelBooks = new List<Book>();
        public SearchPage()
        {
            InitializeComponent();
            returnedBooks = dbHelper.GetReturnedBooksForUser(false);
            unReturnedBooks = dbHelper.GetUnreturnedBooksForUser(false);
            unavailabelBooks = dbHelper.GetUnavailableBooksExcept(returnedBooks, unReturnedBooks);
            List<Book> books = dbHelper.GetAllBooksExcept(returnedBooks, unReturnedBooks, unavailabelBooks);

            if(!returnedBooks.Any() && !unReturnedBooks.Any())
            {
                TextYourBooks.Visibility = Visibility.Collapsed;
                ReturnedBooksListBox.Visibility = Visibility.Collapsed;
                UnreturnedBooksListBox.Visibility = Visibility.Collapsed;
            } else
            {
                ReturnedBooksListBox.ItemsSource = returnedBooks;
                UnreturnedBooksListBox.ItemsSource = unReturnedBooks;
            }

            if (!unavailabelBooks.Any()) 
            { 
                TextUnavailableBooks.Visibility = Visibility.Collapsed;
                UnavailabeBooksListBox.Visibility = Visibility.Collapsed;
            } else UnavailabeBooksListBox.ItemsSource = unavailabelBooks;

            AvailableBooksListBox.ItemsSource = books;
        }

        private void BooksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Book selectedBook)
            {
                BookPage bookPage = new BookPage(selectedBook, returnedBooks, unReturnedBooks);
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToPage(bookPage);
                }
            }
        }
    }
}
