using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public string Annotation { get; set; }
        public int YearPublished { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public int PageCount { get; set; }
        public int ReadingRoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }

        // Конструктор по умолчанию
        public Book() { }

        // Метод для проверки доступности книги
        public bool CheckAvailability()
        {
            return IsAvailable;
        }
    }
}
