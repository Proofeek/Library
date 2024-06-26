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

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MyBooksPage.xaml
    /// </summary>
    public partial class MyBooksPage : Page
    {
        private List<Book> returnedBooks = new List<Book>();
        private List<Book> unReturnedBooks = new List<Book>();
        public MyBooksPage()
        {
            InitializeComponent();

            unReturnedBooks = AppServices.DbHelper.GetUnreturnedBooksForUser(true);
            UnreturnedBooksListBox.ItemsSource = unReturnedBooks;

            returnedBooks = AppServices.DbHelper.GetReturnedBooksForUser(true);
            ReturnedBooksListBox.ItemsSource = returnedBooks;
        }

        private void BooksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Book selectedBook)
            {
                BookPage bookPage = new BookPage(selectedBook, unReturnedBooks, returnedBooks);
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToPage(bookPage);
                }
            }
        }
    }
}
