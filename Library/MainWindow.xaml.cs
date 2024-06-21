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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var searchPage = new SearchPage();
            NavigateToPage(searchPage);
        }

        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }

        public void OpenLogInWindow()
        {
            var logInWindow = new LogInWindow();
            logInWindow.ShowDialog();
        }
    }
}