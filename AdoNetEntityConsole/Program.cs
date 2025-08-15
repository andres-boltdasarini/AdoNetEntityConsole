using System;
using System.Linq;
using ElectronicLibrary;

class Program
{
    static void Main()
    {
        using (var db = new AppContext())
        {
            var userRepo = new UserRepository(db);
            var bookRepo = new BookRepository(db);
            var authorRepo = new AuthorRepository(db);
            var genreRepo = new GenreRepository(db);

            var author1 = new Author { Name = "Михаил Булгаков" };
            var author2 = new Author { Name = "Лев Толстой" };
            authorRepo.Add(author1);
            authorRepo.Add(author2);

            var genre1 = new Genre { Name = "Роман" };
            var genre2 = new Genre { Name = "Фантастика" };
            genreRepo.Add(genre1);
            genreRepo.Add(genre2);

            var book1 = new Book { Title = "Мастер и Маргарита", PublicationYear = 1966 };
            var book2 = new Book { Title = "Война и мир", PublicationYear = 1869 };
            bookRepo.Add(book1);
            bookRepo.Add(book2);

            bookRepo.AddAuthorToBook(book1.Id, author1.Id); 
            bookRepo.AddAuthorToBook(book2.Id, author2.Id);

            bookRepo.AddGenreToBook(book1.Id, genre1.Id); 
            bookRepo.AddGenreToBook(book1.Id, genre2.Id); 
            bookRepo.AddGenreToBook(book2.Id, genre1.Id); 

            var user = new User { Name = "Иван Иванов", Email = "ivan@example.com" };
            userRepo.Add(user);

            userRepo.BorrowBook(user.Id, book1.Id);

            var userWithBooks = userRepo.GetById(user.Id);
            Console.WriteLine($"Пользователь: {userWithBooks.Name}");
            Console.WriteLine("Книги на руках:");
            foreach (var book in userWithBooks.BorrowedBooks)
            {
                Console.WriteLine($"- {book.Title} ({book.PublicationYear})");
                Console.WriteLine("  Авторы: " + string.Join(", ", book.Authors.Select(a => a.Name)));
                Console.WriteLine("  Жанры: " + string.Join(", ", book.Genres.Select(g => g.Name)));
            }
            userRepo.ReturnBook(user.Id, book1.Id);
        }
    }
}