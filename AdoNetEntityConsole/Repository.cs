using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicLibrary
{
    // Репозиторий для работы с пользователями
    public class UserRepository
    {
        private readonly AppContext _context;

        public UserRepository(AppContext context)
        {
            _context = context;
        }

        // Получение пользователя по ID
        public User GetById(int id)
        {
            return _context.Users
                .Include(u => u.BookLoans)
                .ThenInclude(bl => bl.Book)
                .FirstOrDefault(u => u.Id == id);
        }

        // Получение всех пользователей
        public List<User> GetAll()
        {
            return _context.Users
                .Include(u => u.BookLoans)
                .ThenInclude(bl => bl.Book)
                .ToList();
        }

        // Добавление пользователя
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddRange(User user, User user2)
        {
            _context.Users.AddRange(user, user2);
            _context.SaveChanges();
        }

        // Удаление пользователя
        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // Обновление имени пользователя
        public void UpdateUserName(int id, string newName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Name = newName;
                _context.SaveChanges();
            }
        }
    }

    // Репозиторий для работы с книгами
    public class BookRepository
    {
        private readonly AppContext _context;

        public BookRepository(AppContext context)
        {
            _context = context;
        }

        // Получение книги по ID
        public Book GetById(int id)
        {
            return _context.Books
                .Include(b => b.Loans)
                .ThenInclude(bl => bl.User)
                .FirstOrDefault(b => b.Id == id);
        }

        // Получение всех книг
        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Loans)
                .ThenInclude(bl => bl.User)
                .ToList();
        }

        // Добавление книги
        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        // Удаление книги
        public void Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        // Обновление года выпуска книги
        public void UpdateBookYear(int id, int newYear)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.PublicationYear = newYear;
                _context.SaveChanges();
            }
        }
    }
}