using Moq;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _repository;

        public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

        public UserUpdateOnlyRepositoryBuilder GetById(User user)
        {
            _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);
            return this;
        }

        public IUserUpdateOnlyRepository Build() => _repository.Object;
    }
}
