﻿using Npgsql;
using System;
using System.Collections.Generic;
using Library;

namespace Library
{
    public class DataBaseService
    {
        private readonly string connectionString;

        public DataBaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            // Словари для кеширования объектов
            Dictionary<int, Genre> genresCache = new Dictionary<int, Genre>();
            Dictionary<int, Author> authorsCache = new Dictionary<int, Author>();
            Dictionary<int, Publisher> publishersCache = new Dictionary<int, Publisher>();

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
    }
}
