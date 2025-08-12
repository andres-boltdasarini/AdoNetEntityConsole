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

    }
}