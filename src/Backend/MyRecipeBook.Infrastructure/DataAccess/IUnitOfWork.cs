namespace MyRecipeBook.Infrastructure.DataAccess
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}