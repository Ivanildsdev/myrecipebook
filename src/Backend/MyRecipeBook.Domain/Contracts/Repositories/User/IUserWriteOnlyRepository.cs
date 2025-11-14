namespace MyRecipeBook.Domain.Contracts.Repositories.User
{
    public interface IUserWriteOnlyRepository : IRepository
    {
        Task AddAsync(Entities.User user);
    }
}
