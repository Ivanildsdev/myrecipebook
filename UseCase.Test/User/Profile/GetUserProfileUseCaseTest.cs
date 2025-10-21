using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedBuilder;
using CommonTestUtilities.Mapper;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Profile;

namespace UseCase.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //assert
            (var user, var _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            //act
            var result = await useCase.Execute();
            //assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }


    }
}
