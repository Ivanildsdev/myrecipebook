using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
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

    }
}
