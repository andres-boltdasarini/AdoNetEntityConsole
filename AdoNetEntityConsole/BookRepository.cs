using Microsoft.EntityFrameworkCore;

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

        book.Authors.Add(author);
        _context.SaveChanges();
    }

    public void AddGenreToBook(int bookId, int genreId)
    {
        var book = _context.Books.Include(b => b.Genres).First(b => b.Id == bookId);
        var genre = _context.Genres.First(g => g.Id == genreId);

        book.Genres.Add(genre);
        _context.SaveChanges();
    }

    public List<Book> GetBooksByGenre(int genreId)
    {
        return _context.Books
            .Where(b => b.Genres.Any(g => g.Id == genreId))
            .ToList();
    }
}