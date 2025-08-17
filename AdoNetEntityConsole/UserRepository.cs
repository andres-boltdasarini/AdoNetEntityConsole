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

        public User GetById(int id) => _context.Users
            .Include(u => u.BorrowedBooks)
            .FirstOrDefault(u => u.Id == id);

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void BorrowBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.BorrowedBooks).First(u => u.Id == userId);
            var book = _context.Books.First(b => b.Id == bookId);

            if (user != null && book != null)
            {
                user.BorrowedBooks.Add(book);
                _context.SaveChanges();
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.BorrowedBooks).First(u => u.Id == userId);
            var book = user.BorrowedBooks.First(b => b.Id == bookId);

            if (book != null)
            {
                user.BorrowedBooks.Remove(book);
                _context.SaveChanges();
            }
        }

        public bool IsBookBorrowedByUser(int userId, int bookId)
        {
            return _context.Users
                .Any(u => u.Id == userId && u.BorrowedBooks.Any(b => b.Id == bookId));
        }

        public int GetBorrowedBooksCountByUser(int userId)
        {
            return _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.BorrowedBooks.Count)
                .FirstOrDefault();
        }
    }
}