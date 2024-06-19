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

            testGenre = new Genre()
            {
                GenreId = 5,
                GenreName = "Роман"
            };
            testAuthor = new Author()
            {
                AuthorId = 20,
                FirstName = "Михаил",
                LastName = "Булгаков",
                MiddleName = "Афанасьевич"
            };
            testPublisher = new Publisher()
            {
                PublisherId = 9,
                PublisherName = "МОСКВА"
            };

            testBook = new Book()
            {
                BookId = 25,
                Title = "Мастер и Маргарита",
                Genre = testGenre,
                Annotation = "Жарким майским вечером председатель правления МАССОЛИТ Михаил Берлиоз и молодой поэт Иван Бездомный отправились на Патриаршие пруды, чтобы обсудить: существовал ли Иисус Христос. Беседой литераторов заинтересовался некий импозантный гражданин-иностранец. Профессор Воланд стал уверять новых знакомых, что лично присутствовал на допросе бродяги Иешуа Га-Ноцри, который проводил Понтий Пилат.",
                YearPublished = 1966,
                Author = testAuthor,
                Publisher = testPublisher,
                PageCount = 528,
                ReadingRoomNumber = 3,
                IsAvailable = true,
                ImageUrl = "https://www.moscowbooks.ru/image/book/708/orig/i708783.jpg?cu=20201216154536"
            };

            DataContext = this;
        }
    }
}