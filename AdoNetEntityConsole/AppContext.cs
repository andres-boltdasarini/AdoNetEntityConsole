using Microsoft.EntityFrameworkCore;

namespace AdoNetEntityConsole
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=ent;Username=postgres;Password=1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Основная таблица
            modelBuilder.Entity<User>().ToTable("users");

            // Автоматическое преобразование в нижний регистр
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }
            }
        }
    }
}