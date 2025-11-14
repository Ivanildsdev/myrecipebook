using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedBuilder;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.MyExceptionBase;

namespace UseCase.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //assert
            (var user, _) = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);
            Func<Task> act = async () => await useCase.Execute(request);
            //act
            await act.Should().NotThrowAsync(); ;
            //assert
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            //assert
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => { await useCase.Execute(request); };
            // act / assert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 && 
                       e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            
            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            //assert
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request);  };
            // act / assert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 && 
                       e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);
        }

        private UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            var userReadOnlyRepoBuilder = new UserReadOnlyRepositoryBuilder();
            if (!string.IsNullOrEmpty(email))
                userReadOnlyRepoBuilder.ExistActiveUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepoBuilder.Build(), unitOfWork);
        }


    }
}
