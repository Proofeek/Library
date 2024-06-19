using Library;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Book> books;
        public ObservableCollection<Book> Books
        {
            get { return books; }
            set
            {
                books = value;
                OnPropertyChanged();
            }
        }

        public BooksViewModel()
        {
            LoadBooks();
        }

        private void LoadBooks()
        {
            string connectionString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";
            DataBaseService dbHelper = new DataBaseService(connectionString);
            List<Book> booksFromDb = dbHelper.GetAllBooks();
            Books = new ObservableCollection<Book>(booksFromDb);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
