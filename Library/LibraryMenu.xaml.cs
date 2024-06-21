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
    /// Логика взаимодействия для LibraryMenu.xaml
    /// </summary>
    public partial class LibraryMenu : UserControl
    {
        public LibraryMenu()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchPage = new SearchPage();

            // Find the parent window and call the NavigateToPage method
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToPage(searchPage);
            }
        }

        private void MyBooksButton_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void AccountButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.OpenLogInWindow();
            }
        }
    }
}
