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
            }
        }
    }
}