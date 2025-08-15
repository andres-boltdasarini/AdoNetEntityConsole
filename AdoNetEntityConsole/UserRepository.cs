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

            user.BorrowedBooks.Add(book);
            _context.SaveChanges();
        }

        public void ReturnBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.BorrowedBooks).First(u => u.Id == userId);
            var book = user.BorrowedBooks.First(b => b.Id == bookId);

            user.BorrowedBooks.Remove(book);
            _context.SaveChanges();
        }
    }
}