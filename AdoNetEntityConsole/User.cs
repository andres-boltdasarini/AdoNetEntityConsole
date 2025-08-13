using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary
{
    // Пользователь
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        // Связь с книгами, которые пользователь взял
        public List<Book> BorrowedBooks { get; set; } = new List<Book>();
    }

    // Книга
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        
        // Связь с пользователем, который взял книгу
        public int? UserId { get; set; }
        public User User { get; set; }
        
        // Связь с авторами (многие ко многим)
        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        
        // Связь с жанрами (многие ко многим)
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
    
    // Автор
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        // Связь с книгами (многие ко многим)
        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
    
    // Жанр
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        // Связь с книгами (многие ко многим)
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
    
    // Промежуточная таблица для связи книг и авторов
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
    
    // Промежуточная таблица для связи книг и жанров
    public class BookGenre
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}