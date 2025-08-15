public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Book> BorrowedBooks { get; set; } = new();
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }

    public int? UserId { get; set; }
    public User User { get; set; }

    public List<Author> Authors { get; set; } = new();

    public List<Genre> Genres { get; set; } = new();
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Book> Books { get; set; } = new();
}

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Book> Books { get; set; } = new();
}