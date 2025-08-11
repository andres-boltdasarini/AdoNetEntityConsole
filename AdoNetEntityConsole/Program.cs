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
                // Создаем тестовые данные
                var user1 = new User { Name = "Иван Иванов", Email = "ivan@example.com" };
                var user2 = new User { Name = "Мария Петрова", Email = "maria@example.com" };

                var book1 = new Book { Title = "Война и мир", PublicationYear = 1869 };
                var book2 = new Book { Title = "Преступление и наказание", PublicationYear = 1866 };
                var book3 = new Book { Title = "1984", PublicationYear = 1949 };

                db.Users.AddRange(user1, user2);
                db.Books.AddRange(book1, book2, book3);
                db.SaveChanges();

                // Выдаем книги
                var loan1 = new BookLoan { UserId = user1.Id, BookId = book1.Id };
                var loan2 = new BookLoan { UserId = user1.Id, BookId = book2.Id };
                var loan3 = new BookLoan { UserId = user2.Id, BookId = book3.Id };

                db.BookLoans.AddRange(loan1, loan2, loan3);
                db.SaveChanges();

                // Возвращаем одну книгу
                loan1.ReturnDate = DateTime.UtcNow;
                db.SaveChanges();

                // Выводим информацию
                Console.WriteLine("Пользователи и их книги:");
                foreach (var user in db.Users.Include(u => u.BookLoans).ThenInclude(bl => bl.Book))
                {
                    Console.WriteLine($"{user.Name} ({user.Email}):");
                    foreach (var loan in user.BookLoans)
                    {
                        Console.WriteLine($"- {loan.Book.Title} ({loan.Book.PublicationYear}) " +
                                          $"[Выдана: {loan.LoanDate:d}, " +
                                          $"{(loan.ReturnDate.HasValue ? "Возвращена: " + loan.ReturnDate.Value.ToString("d") : "На руках")}]");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}     