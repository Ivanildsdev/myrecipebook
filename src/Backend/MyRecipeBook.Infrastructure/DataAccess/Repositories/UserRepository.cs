using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
    {
        private readonly MyRecipeBookDbContext _context;

        public UserRepository(MyRecipeBookDbContext context) => _context = context;

        public async Task AddAsync(User user)
        {
           await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Equals(email) && u.Active);
        }

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _context.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(user => user.Email.Equals(email) &&
                                             user.Password.Equals(password) &&
                                             user.Active.Equals(true));
        }

        public async Task<User> GetById(long id)
        {
            return await _context
                .Users
                .FirstAsync(user => user.Id == id);
        }

        public void Update(User user) => _context.Users.Update(user);
    }
}
