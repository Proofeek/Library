using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{

    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }

    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}".Trim();
            }
        }
    }

    public class Publisher
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
    }

    public class Reader
    {
        public int ReaderId { get; set; }
        public string ReaderName { get; set; }
        public string ReaderEmail { get; set; }
    }

    public class Loan
    {
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
