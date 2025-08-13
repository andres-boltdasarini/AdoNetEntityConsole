using ElectronicLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ElectronicLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext())
            {
                // Создание репозиториев
                var userRepo = new UserRepository(db);
                var bookRepo = new BookRepository(db);
                var authorRepo = new AuthorRepository(db);
                var genreRepo = new GenreRepository(db);

                // Добавление пользователей
                var user1 = new User { Name = "Alice", Email = "alice@example.com" };
                var user2 = new User { Name = "Bob", Email = "bob@example.com" };
                userRepo.AddRange(user1, user2);

                // Добавление авторов
                var author1 = new Author { Name = "Михаил Булгаков" };
                var author2 = new Author { Name = "Лев Толстой" };
                authorRepo.Add(author1);
                authorRepo.Add(author2);

                // Добавление жанров
                var genre1 = new Genre { Name = "Роман" };
                var genre2 = new Genre { Name = "Фантастика" };
                genreRepo.Add(genre1);
                genreRepo.Add(genre2);

                // Добавление книг
                var book1 = new Book { Title = "Мастер и Маргарита", PublicationYear = 1966 };
                var book2 = new Book { Title = "Война и мир", PublicationYear = 1869 };
                bookRepo.Add(book1);
                bookRepo.Add(book2);

                // Добавление авторов к книгам
                bookRepo.AddAuthorToBook(1, 1); // Булгаков -> Мастер и Маргарита
                bookRepo.AddAuthorToBook(2, 2); // Толстой -> Война и мир

                // Добавление жанров к книгам
                bookRepo.AddGenreToBook(1, 1); // Мастер и Маргарита -> Роман
                bookRepo.AddGenreToBook(1, 2); // Мастер и Маргарита -> Фантастика
                bookRepo.AddGenreToBook(2, 1); // Война и мир -> Роман

                // Выдача книги пользователю
                userRepo.BorrowBook(1, 1); // Alice берет Мастер и Маргарита

                // Получение информации о пользователе и его книгах
                var alice = userRepo.GetById(1);
                Console.WriteLine($"Пользователь: {alice.Name}");
                Console.WriteLine("Взятые книги:");
                foreach (var book in alice.BorrowedBooks)
                {
                    Console.WriteLine($"- {book.Title} ({book.PublicationYear})");
                    Console.WriteLine("  Авторы: " + string.Join(", ", 
                        book.BookAuthors.Select(ba => ba.Author.Name)));
                    Console.WriteLine("  Жанры: " + string.Join(", ", 
                        book.BookGenres.Select(bg => bg.Genre.Name)));
                }

                // Возврат книги
                userRepo.ReturnBook(1);
                var scifiBooks = bookRepo.GetBooksByGenreAndYearRange(genre2.Id, 1960, 1970);

// 2. Получить количество книг Булгакова в библиотеке
var bulgakovBooksCount = bookRepo.GetBooksCountByAuthor(author1.Id);

// 3. Получить количество книг в жанре "Роман"
var novelBooksCount = bookRepo.GetBooksCountByGenre(genre1.Id);

// 4. Проверить есть ли книга "Мастер и Маргарита" Булгакова в библиотеке
var isMasterExists = bookRepo.IsBookExistsByAuthorAndTitle(author1.Id, "Мастер и Маргарита");

// 5. Проверить есть ли книга с ID=1 на руках у пользователя с ID=1
var isBookBorrowed = userRepo.IsBookBorrowedByUser(1, 1);

// 6. Получить количество книг на руках у пользователя с ID=1
var borrowedCount = userRepo.GetBorrowedBooksCountByUser(1);

// 7. Получить последнюю вышедшую книгу
var latestBook = bookRepo.GetLatestPublishedBook();

// 8. Получить все книги, отсортированные по названию
var booksByTitle = bookRepo.GetAllBooksOrderedByTitle();

// 9. Получить все книги, отсортированные по году выхода (по убыванию)
var booksByYear = bookRepo.GetAllBooksOrderedByYearDesc();

foreach (var book in booksByYear){
            Console.WriteLine($"Название: {book.Title}");
        Console.WriteLine($"Год издания: {book.PublicationYear}");
}
            }
        }
    }
}