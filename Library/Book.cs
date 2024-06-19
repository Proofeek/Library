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
        public int GenreId { get; set; }
        public string Annotation { get; set; }
        public int YearPublished { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int PageCount { get; set; }
        public int ReadingRoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }

        // Конструктор по умолчанию
        public Book() { }

        // Конструктор с параметрами
        public Book(int bookId, string title, int genreId, string annotation, int yearPublished, int authorId, int publisherId, int pageCount, int readingRoomNumber, bool isAvailable, string imageUrl)
        {
            BookId = bookId;
            Title = title;
            GenreId = genreId;
            Annotation = annotation;
            YearPublished = yearPublished;
            AuthorId = authorId;
            PublisherId = publisherId;
            PageCount = pageCount;
            ReadingRoomNumber = readingRoomNumber;
            IsAvailable = isAvailable;
            ImageUrl = imageUrl;
        }

        // Метод для получения строки с описанием книги
        public override string ToString()
        {
            return $"ID: {BookId}, Title: {Title}, GenreId: {GenreId}, Year: {YearPublished}, AuthorId: {AuthorId}, PublisherId: {PublisherId}, Pages: {PageCount}, Room: {ReadingRoomNumber}, Available: {IsAvailable}, Image: {ImageUrl}";
        }

        // Метод для проверки доступности книги
        public bool CheckAvailability()
        {
            return IsAvailable;
        }
    }
}
