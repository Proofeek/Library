using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

            if (Book.Genre.GenreId == 2 ||
                Book.Genre.GenreId == 3 ||
                Book.Genre.GenreId == 4 ||
                Book.Genre.GenreId == 10)
            {
                // Заполнение дополнительной литературы
                List<Book> additionalLiterature = dbHelper.FindSimilarBooks(Book);
                if (additionalLiterature.Count > 0) CC1.Content = additionalLiterature[0];
                else
                {
                    CC1.Visibility = Visibility.Collapsed;
                    TextAdditional.Visibility = Visibility.Collapsed;
                }
                CC2.Content = additionalLiterature.Count > 1 ? additionalLiterature[1] : CC2.Visibility = Visibility.Collapsed;
                CC3.Content = additionalLiterature.Count > 2 ? additionalLiterature[2] : CC3.Visibility = Visibility.Collapsed;
                TextSimilar.Visibility = Visibility.Collapsed;
            }
            else
            {
                CC1.Visibility = Visibility.Collapsed;
                CC2.Visibility = Visibility.Collapsed;
                CC3.Visibility = Visibility.Collapsed;
                TextAdditional.Visibility = Visibility.Collapsed;
                // Заполнение похожей литературы
                List<Book> similarBooks = dbHelper.FindSimilarBooks(Book);
                if (similarBooks.Any()) SimilarListBox.ItemsSource = similarBooks;
                else TextSimilar.Visibility = Visibility.Collapsed;
            }



            this.DataContext = this;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SimilarListBox.SelectedItem is Book selectedBook)
            {
                BookPage bookPage = new BookPage(selectedBook);
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToPage(bookPage);
                }
            }
        }

        private void ContentControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContentControl contentControl)
            {
                if (contentControl.Content is Book selectedBook)
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
}
