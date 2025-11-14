using CommonTestUtilities.Criptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.MyExceptionBase;

namespace UseCase.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //assert
            (var user,  var password) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            //act
            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });
            //assert
            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            //assert
            var request = RequestLoginJsonBuilder.Build();
            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);
            // act / assert
            (await act.Should().ThrowAsync<InvalidLoginException>())
                .Where(e => e.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));

        }


        private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
        {
            var encripter = PasswordEncripterBuilder.Build();
            var readOnlyRepoBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (user is not null)
                readOnlyRepoBuilder.GetByEmailAndPassword(user);

            return new DoLoginUseCase(readOnlyRepoBuilder.Build(),  encripter, accessTokenGenerator);
        }

    }
}
