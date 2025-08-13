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
            public bool IsBookBorrowedByUser(int userId, int bookId)
    {
        return _context.Users
            .Include(u => u.BorrowedBooks)
            .Any(u => u.Id == userId && u.BorrowedBooks.Any(b => b.Id == bookId));
    }

    // 6. Получать количество книг на руках у пользователя
    public int GetBorrowedBooksCountByUser(int userId)
    {
        return _context.Users
            .Include(u => u.BorrowedBooks)
            .FirstOrDefault(u => u.Id == userId)?
            .BorrowedBooks.Count ?? 0;
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
    var exists = _context.BookAuthors
        .Any(ba => ba.BookId == bookId && ba.AuthorId == authorId);
    
    if (!exists)
    {
        _context.BookAuthors.Add(new BookAuthor { BookId = bookId, AuthorId = authorId });
        _context.SaveChanges();
    }
}
        
        // Добавить жанр к книге
public void AddGenreToBook(int bookId, int genreId)
{
    var exists = _context.BookGenres
        .Any(bg => bg.BookId == bookId && bg.GenreId == genreId);
    
    if (!exists)
    {
        _context.BookGenres.Add(new BookGenre { BookId = bookId, GenreId = genreId });
        _context.SaveChanges();
    }
}
            public List<Book> GetBooksByGenreAndYearRange(int genreId, int startYear, int endYear)
    {
        return _context.Books
            .Include(b => b.BookGenres)
            .Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId)) 
            .Where(b => b.PublicationYear >= startYear && b.PublicationYear <= endYear)
            .ToList();
    }

    // 2. Получать количество книг определенного автора в библиотеке
    public int GetBooksCountByAuthor(int authorId)
    {
        return _context.BookAuthors
            .Count(ba => ba.AuthorId == authorId);
    }

    // 3. Получать количество книг определенного жанра в библиотеке
    public int GetBooksCountByGenre(int genreId)
    {
        return _context.BookGenres
            .Count(bg => bg.GenreId == genreId);
    }

    // 4. Проверять есть ли книга определенного автора и с определенным названием в библиотеке
    public bool IsBookExistsByAuthorAndTitle(int authorId, string title)
    {
        return _context.Books
            .Include(b => b.BookAuthors)
            .Any(b => b.Title == title && b.BookAuthors.Any(ba => ba.AuthorId == authorId));
    }

    // 7. Получение последней вышедшей книги
    public Book GetLatestPublishedBook()
    {
        return _context.Books
            .OrderByDescending(b => b.PublicationYear)
            .FirstOrDefault();
    }

    // 8. Получение списка всех книг, отсортированного в алфавитном порядке по названию
    public List<Book> GetAllBooksOrderedByTitle()
    {
        return _context.Books
            .OrderBy(b => b.Title)
            .ToList();
    }

    // 9. Получение списка всех книг, отсортированного в порядке убывания года их выхода
    public List<Book> GetAllBooksOrderedByYearDesc()
    {
        return _context.Books
            .OrderByDescending(b => b.PublicationYear)
            .ToList();
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