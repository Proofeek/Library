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
        public MyBooksPage()
        {
            InitializeComponent();
            List<Book> unreternedBooks = AppServices.DbHelper.GetUnreturnedBooksForUser();
            UnreturnedBooksListBox.ItemsSource = unreternedBooks;

            List<Book> reternedBooks = AppServices.DbHelper.GetReturnedBooksForUser();
            ReturnedBooksListBox.ItemsSource = reternedBooks;
        }

        private void BooksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReturnedBooksListBox.SelectedItem is Book selectedBook)
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
