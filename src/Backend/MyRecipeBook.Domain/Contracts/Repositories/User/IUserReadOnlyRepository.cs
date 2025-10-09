namespace MyRecipeBook.Domain.Contracts.Repositories.User
{
    public interface IUserReadOnlyRepository : IRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);

    }
}
