using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Contracts.Repositories.User
{
    public interface IUserReadOnlyRepository : IRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
        Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
        Task<Entities.User?> GetByEmailAndPassword(string email, string password);


    }
}
