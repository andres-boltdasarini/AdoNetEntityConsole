using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ElectronicLibrary
{
    // Пользователь
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    }

    // Книга
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public List<BookLoan> Loans { get; set; } = new List<BookLoan>();
    }

    // Выдача книги пользователю
    public class BookLoan
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }

    // Контекст базы данных
    

   
}