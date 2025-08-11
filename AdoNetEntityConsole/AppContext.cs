using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary
{
   public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        public AppContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=ent;Username=postgres;Password=1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка имен таблиц в нижнем регистре для PostgreSQL
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Book>().ToTable("books");
            modelBuilder.Entity<BookLoan>().ToTable("book_loans");

            // Уникальность email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Связи для выдачи книг
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.User)
                .WithMany(u => u.BookLoans)
                .HasForeignKey(bl => bl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(bl => bl.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}