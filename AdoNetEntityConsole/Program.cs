using ElectronicLibrary;
using Microsoft.EntityFrameworkCore;


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

                // Добавление пользователя
                var newUser = new User { Name = "Алексей Сидоров", Email = "alex@example.com" };
                userRepo.Add(newUser);

                    var user1 = new User { Name = "Alice", Email = "alex@example.com" };
    var user2 = new User { Name = "Bob", Email = "alex@example.com" };
    var user3 = new User { Name = "Bruce", Email = "alex@example.com" };

    userRepo.AddRange(user2, user3);

                // Обновление имени пользователя
                userRepo.UpdateUserName(1, "Иван Иванович Иванов");

                // Добавление книги
                var newBook = new Book { Title = "Мастер и Маргарита", PublicationYear = 1966 };
                bookRepo.Add(newBook);

                // Обновление года книги
                bookRepo.UpdateBookYear(1, 1870);

                // Получение книги по ID
                var book = bookRepo.GetById(1);
                Console.WriteLine($"Обновленная книга: {book.Title} ({book.PublicationYear})");

                // Удаление книги
                bookRepo.Delete(bookRepo.GetById(3));
            }
        }
    }
}     