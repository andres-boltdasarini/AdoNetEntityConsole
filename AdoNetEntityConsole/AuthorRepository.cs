namespace ElectronicLibrary
{
    public class AuthorRepository
    {
        private readonly AppContext _context;
        public AuthorRepository(AppContext context) => _context = context;

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }
    }
}