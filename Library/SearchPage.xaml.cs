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
        public SearchPage()
        {
            InitializeComponent();
            List<Book> returnedBooks = dbHelper.GetReturnedBooksForUser();
            List<Book> unReturnedBooks = dbHelper.GetUnreturnedBooksForUser();
            List<Book> unavailabelBooks = dbHelper.GetUnavailableBooksExcept(returnedBooks, unReturnedBooks);
            List<Book> books = dbHelper.GetAllBooksExcept(returnedBooks, unReturnedBooks, unavailabelBooks);

            BooksListBox.ItemsSource = books;
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
