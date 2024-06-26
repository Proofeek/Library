using Npgsql;
using System;
using System.Collections.Generic;
using Library;
using System.Windows;

namespace Library
{
    public class DataBaseService
    {
        private readonly string connectionString;

        public DataBaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        // Словари для кеширования объектов
        Dictionary<int, Genre> genresCache = new Dictionary<int, Genre>();
        Dictionary<int, Author> authorsCache = new Dictionary<int, Author>();
        Dictionary<int, Publisher> publishersCache = new Dictionary<int, Publisher>();

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();



            string query = @"
                SELECT 
                    b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
                    g.GenreId, g.GenreName,
                    a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
                    p.PublisherId, p.PublisherName
                FROM 
                    Books b
                JOIN 
                    Genres g ON b.GenreId = g.GenreId
                JOIN 
                    Authors a ON b.AuthorId = a.AuthorId
                JOIN 
                    Publishers p ON b.PublisherId = p.PublisherId";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public List<Book> FindSimilarBooks(Book book)
        {
            List<Book> similarBooks = new List<Book>();

            string query = @"
                SELECT 
                    b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
                    g.GenreId, g.GenreName,
                    a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
                    p.PublisherId, p.PublisherName
                FROM 
                    Books b
                JOIN 
                    Genres g ON b.GenreId = g.GenreId
                JOIN 
                    Authors a ON b.AuthorId = a.AuthorId
                JOIN 
                    Publishers p ON b.PublisherId = p.PublisherId
                WHERE 
                    b.BookId != @BookId
                    AND (
                        g.GenreName = @GenreName
                        OR b.Title LIKE @TitleSearch
                        OR a.LastName = @AuthorLastName
                    )
                LIMIT 3";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", book.BookId);
                    command.Parameters.AddWithValue("@GenreName", book.Genre.GenreName);
                    command.Parameters.AddWithValue("@TitleSearch", $"%{book.Title}%");
                    command.Parameters.AddWithValue("@AuthorLastName", book.Author.LastName);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book similarBook = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            similarBooks.Add(similarBook);
                        }
                    }
                }
            }

            return similarBooks;
        }
        public bool LoadReaderByEmail(string email)
        {
            string query = @"
                SELECT 
                    ReaderId, FirstName, MiddleName, LastName, readeremail
                FROM 
                    Readers
                WHERE 
                    readeremail = @readeremail";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@readeremail", email);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Записываем данные в статический класс User
                            User.Id = reader.GetInt32(reader.GetOrdinal("ReaderId"));
                            User.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            User.MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName"));
                            User.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            User.Email = reader.GetString(reader.GetOrdinal("readeremail"));

                            
                            return true;
                        }
                        else
                        {
                            // В случае, если читатель с таким email не найден, можно обработать ошибку или сделать что-то еще
                            
                            return false;
                            //throw new ArgumentException("Читатель с указанным email не найден.");
                        }
                    }
                }
            }
        }
        public List<Book> GetReturnedBooksForUser(bool includeLoanDates)
        {
            List<Book> returnedBooks = new List<Book>();

            string query = @"
        SELECT 
            b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
            g.GenreId, g.GenreName,
            a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
            p.PublisherId, p.PublisherName,
            l.LoanDate, l.ReturnDate
        FROM 
            Loans l
        JOIN 
            Books b ON l.BookId = b.BookId
        JOIN 
            Genres g ON b.GenreId = g.GenreId
        JOIN 
            Authors a ON b.AuthorId = a.AuthorId
        JOIN 
            Publishers p ON b.PublisherId = p.PublisherId
        WHERE 
            l.readerId = @readerId
            AND l.ReturnDate IS NOT NULL";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@readerId", User.Id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            if (includeLoanDates)
                            {
                                DateTime loanDate = reader.GetDateTime(reader.GetOrdinal("LoanDate"));
                                DateTime returnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
                                book.ShortAnnotation = $"Выдача: {loanDate.ToShortDateString()}\nВозврат: {returnDate.ToShortDateString()}";
                            }

                            returnedBooks.Add(book);
                        }
                    }
                }
            }

            return returnedBooks;
        }
        public List<Book> GetUnreturnedBooksForUser(bool includeLoanDates)
        {
            List<Book> unreturnedBooks = new List<Book>();

            string query = @"
        SELECT 
            b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
            g.GenreId, g.GenreName,
            a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
            p.PublisherId, p.PublisherName,
            l.LoanDate
        FROM 
            Loans l
        JOIN 
            Books b ON l.BookId = b.BookId
        JOIN 
            Genres g ON b.GenreId = g.GenreId
        JOIN 
            Authors a ON b.AuthorId = a.AuthorId
        JOIN 
            Publishers p ON b.PublisherId = p.PublisherId
        WHERE 
            l.readerId = @readerId
            AND l.ReturnDate IS NULL";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@readerId", User.Id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            if (includeLoanDates)
                            {
                                DateTime loanDate = reader.GetDateTime(reader.GetOrdinal("LoanDate"));
                                book.ShortAnnotation = $"Выдача: {loanDate.ToShortDateString()}";
                            }

                            unreturnedBooks.Add(book);
                        }
                    }
                }
            }

            return unreturnedBooks;
        }
        public List<Book> GetAllBooksExcept(List<Book> excludedBooks1, List<Book> excludedBooks2, List<Book> excludedBooks3)
        {
            List<Book> books = new List<Book>();

            // Создание списка всех исключаемых книг
            List<int> allExcludedBookIds = new List<int>();
            allExcludedBookIds.AddRange(excludedBooks1.Select(b => b.BookId));
            allExcludedBookIds.AddRange(excludedBooks2.Select(b => b.BookId));
            allExcludedBookIds.AddRange(excludedBooks3.Select(b => b.BookId));

            // Формирование плейсхолдеров для параметров
            string excludedBooksPlaceholders = string.Join(", ", allExcludedBookIds.Select((id, index) => $"@excludedBook{index}"));

            // Формирование строки запроса
            string query = $@"
        SELECT 
            b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
            g.GenreId, g.GenreName,
            a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
            p.PublisherId, p.PublisherName
        FROM 
            Books b
        JOIN 
            Genres g ON b.GenreId = g.GenreId
        JOIN 
            Authors a ON b.AuthorId = a.AuthorId
        JOIN 
            Publishers p ON b.PublisherId = p.PublisherId
        WHERE 
            b.BookId NOT IN ({excludedBooksPlaceholders})";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    // Добавление параметров для каждого исключаемого идентификатора книги
                    for (int i = 0; i < allExcludedBookIds.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@excludedBook{i}", allExcludedBookIds[i]);
                    }

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }
        public List<Book> GetUnavailableBooksExcept(List<Book> excludedBooks1, List<Book> excludedBooks2)
        {
            List<Book> books = new List<Book>();

            // Создание списка всех исключаемых книг
            List<int> allExcludedBookIds = new List<int>();
            allExcludedBookIds.AddRange(excludedBooks1.Select(b => b.BookId));
            allExcludedBookIds.AddRange(excludedBooks2.Select(b => b.BookId));

            // Формирование плейсхолдеров для параметров
            string excludedBooksPlaceholders = string.Join(", ", allExcludedBookIds.Select((id, index) => $"@excludedBook{index}"));

            // Формирование строки запроса
            string query = $@"
        SELECT 
            b.BookId, b.Title, b.Annotation, b.YearPublished, b.PageCount, b.ReadingRoomNumber, b.IsAvailable, b.ImageUrl,
            g.GenreId, g.GenreName,
            a.AuthorId, a.FirstName, a.MiddleName, a.LastName,
            p.PublisherId, p.PublisherName
        FROM 
            Books b
        JOIN 
            Genres g ON b.GenreId = g.GenreId
        JOIN 
            Authors a ON b.AuthorId = a.AuthorId
        JOIN 
            Publishers p ON b.PublisherId = p.PublisherId
        WHERE 
            b.IsAvailable = false
            AND b.BookId NOT IN ({excludedBooksPlaceholders})";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    // Добавление параметров для каждого исключаемого идентификатора книги
                    for (int i = 0; i < allExcludedBookIds.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@excludedBook{i}", allExcludedBookIds[i]);
                    }

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Кеширование жанра
                            int genreId = reader.GetInt32(reader.GetOrdinal("GenreId"));
                            if (!genresCache.TryGetValue(genreId, out Genre genre))
                            {
                                genre = new Genre
                                {
                                    GenreId = genreId,
                                    GenreName = reader.GetString(reader.GetOrdinal("GenreName"))
                                };
                                genresCache[genreId] = genre;
                            }

                            // Кеширование автора
                            int authorId = reader.GetInt32(reader.GetOrdinal("AuthorId"));
                            if (!authorsCache.TryGetValue(authorId, out Author author))
                            {
                                author = new Author
                                {
                                    AuthorId = authorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader.GetString(reader.GetOrdinal("MiddleName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };
                                authorsCache[authorId] = author;
                            }

                            // Кеширование издателя
                            int publisherId = reader.GetInt32(reader.GetOrdinal("PublisherId"));
                            if (!publishersCache.TryGetValue(publisherId, out Publisher publisher))
                            {
                                publisher = new Publisher
                                {
                                    PublisherId = publisherId,
                                    PublisherName = reader.GetString(reader.GetOrdinal("PublisherName"))
                                };
                                publishersCache[publisherId] = publisher;
                            }

                            // Создание объекта Book
                            Book book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = genre,
                                FullAnnotation = reader.GetString(reader.GetOrdinal("Annotation")),
                                YearPublished = reader.GetInt32(reader.GetOrdinal("YearPublished")),
                                Author = author,
                                Publisher = publisher,
                                PageCount = reader.GetInt32(reader.GetOrdinal("PageCount")),
                                ReadingRoomNumber = reader.GetInt32(reader.GetOrdinal("ReadingRoomNumber")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl"))
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }


    }
}