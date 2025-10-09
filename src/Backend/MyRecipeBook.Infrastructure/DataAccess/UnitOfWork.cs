namespace MyRecipeBook.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyRecipeBookDbContext _context;

        public UnitOfWork(MyRecipeBookDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync() => await _context.SaveChangesAsync();

    }
}
