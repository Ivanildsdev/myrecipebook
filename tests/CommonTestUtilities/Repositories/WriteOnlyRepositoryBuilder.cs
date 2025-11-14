using Moq;
using MyRecipeBook.Domain.Contracts.Repositories.User;

namespace CommonTestUtilities.Repositories
{
    public class UserWriteOnlyRepositoryBuilder
    {
        private readonly Mock<IUserWriteOnlyRepository> _repository;
        public UserWriteOnlyRepositoryBuilder() => _repository = new Mock<IUserWriteOnlyRepository>();

        public IUserWriteOnlyRepository Build() => _repository.Object;
    }
}
