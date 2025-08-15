namespace ElectronicLibrary
{
    public class GenreRepository
    {
        private readonly AppContext _context;
        public GenreRepository(AppContext context) => _context = context;

        public void Add(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
    }
}