using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary
{
    public class BookRepository
    {
        private readonly AppContext _context;

        public BookRepository(AppContext context) => _context = context;

        public Book GetById(int id) => _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .FirstOrDefault(b => b.Id == id);

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void AddAuthorToBook(int bookId, int authorId)
        {
            var book = _context.Books.Include(b => b.Authors).First(b => b.Id == bookId);
            var author = _context.Authors.First(a => a.Id == authorId);

            if (!book.Authors.Any(a => a.Id == authorId))
            {
                book.Authors.Add(author);
                _context.SaveChanges();
            }
        }

        public void AddGenreToBook(int bookId, int genreId)
        {
            var book = _context.Books.Include(b => b.Genres).First(b => b.Id == bookId);
            var genre = _context.Genres.First(g => g.Id == genreId);

            if (!book.Genres.Any(g => g.Id == genreId))
            {
                book.Genres.Add(genre);
                _context.SaveChanges();
            }
        }

        public List<Book> GetBooksByGenreAndYearRange(int genreId, int startYear, int endYear)
        {
            return _context.Books
                .Where(b => b.Genres.Any(g => g.Id == genreId))
                .Where(b => b.PublicationYear >= startYear && b.PublicationYear <= endYear)
                .ToList();
        }

        public int GetBooksCountByAuthor(int authorId)
        {
            return _context.Books
                .Count(b => b.Authors.Any(a => a.Id == authorId));
        }

        public int GetBooksCountByGenre(int genreId)
        {
            return _context.Books
                .Count(b => b.Genres.Any(g => g.Id == genreId));
        }

        public bool IsBookExistsByAuthorAndTitle(int authorId, string title)
        {
            return _context.Books
                .Any(b => b.Title == title && b.Authors.Any(a => a.Id == authorId));
        }

        public Book GetLatestPublishedBook()
        {
            return _context.Books
                .OrderByDescending(b => b.PublicationYear)
                .FirstOrDefault();
        }

        public List<Book> GetAllBooksOrderedByTitle()
        {
            return _context.Books
                .OrderBy(b => b.Title)
                .ToList();
        }

        public List<Book> GetAllBooksOrderedByYearDesc()
        {
            return _context.Books
                .OrderByDescending(b => b.PublicationYear)
                .ToList();
        }
    }
}