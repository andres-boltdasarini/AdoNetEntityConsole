using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicLibrary
{
    public class UserRepository
    {
        private readonly AppContext _context;

        public UserRepository(AppContext context) => _context = context;

        public User GetById(int id) => _context.Users.Include(u => u.BorrowedBooks).FirstOrDefault(u => u.Id == id);
        public List<User> GetAll() => _context.Users.Include(u => u.BorrowedBooks).ToList();
        
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddRange(params User[] users)
        {
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void UpdateUserName(int id, string newName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Name = newName;
                _context.SaveChanges();
            }
        }
        
        // Выдать книгу пользователю
        public void BorrowBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.BorrowedBooks).FirstOrDefault(u => u.Id == userId);
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            
            if (user != null && book != null)
            {
                book.UserId = userId;
                user.BorrowedBooks.Add(book);
                _context.SaveChanges();
            }
        }
        
        // Вернуть книгу
        public void ReturnBook(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                book.UserId = null;
                _context.SaveChanges();
            }
        }
    }

    public class BookRepository
    {
        private readonly AppContext _context;

        public BookRepository(AppContext context) => _context = context;

        public Book GetById(int id) => _context.Books
            .Include(b => b.User)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .FirstOrDefault(b => b.Id == id);

        public List<Book> GetAll() => _context.Books
            .Include(b => b.User)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .ToList();

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public void UpdateBookYear(int id, int newYear)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.PublicationYear = newYear;
                _context.SaveChanges();
            }
        }
        
        // Добавить автора к книге
        public void AddAuthorToBook(int bookId, int authorId)
        {
            var book = _context.Books.Include(b => b.BookAuthors).FirstOrDefault(b => b.Id == bookId);
            var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
            
            if (book != null && author != null)
            {
                book.BookAuthors.Add(new BookAuthor { BookId = bookId, AuthorId = authorId });
                _context.SaveChanges();
            }
        }
        
        // Добавить жанр к книге
        public void AddGenreToBook(int bookId, int genreId)
        {
            var book = _context.Books.Include(b => b.BookGenres).FirstOrDefault(b => b.Id == bookId);
            var genre = _context.Genres.FirstOrDefault(g => g.Id == genreId);
            
            if (book != null && genre != null)
            {
                book.BookGenres.Add(new BookGenre { BookId = bookId, GenreId = genreId });
                _context.SaveChanges();
            }
        }
    }
    
    public class AuthorRepository
    {
        private readonly AppContext _context;
        
        public AuthorRepository(AppContext context) => _context = context;
        
        public Author GetById(int id) => _context.Authors
            .Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
            .FirstOrDefault(a => a.Id == id);
            
        public List<Author> GetAll() => _context.Authors
            .Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
            .ToList();
            
        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }
        
        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
    
    public class GenreRepository
    {
        private readonly AppContext _context;
        
        public GenreRepository(AppContext context) => _context = context;
        
        public Genre GetById(int id) => _context.Genres
            .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
            .FirstOrDefault(g => g.Id == id);
            
        public List<Genre> GetAll() => _context.Genres
            .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
            .ToList();
            
        public void Add(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
        
        public void Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
        }
    }
}