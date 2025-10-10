using CommonTestUtilities.Criptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.MyExceptionBase;

namespace UseCase.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //assert
            var useCase = CreateUseCase();
            var request = RequestRegisterUserJsonBuilder.Build();
            //act
            var result = await useCase.Execute(request);
            //assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            //assert
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);
            // act / assert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            //assert
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);
            // act / assert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));

        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var encripter = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readOnlyRepoBuilder = new UserReadOnlyRepositoryBuilder();

            if (!string.IsNullOrEmpty(email))
                readOnlyRepoBuilder.ExistActiveUserWithEmail(email);

            var writeOnlyRepo = new UserWriteOnlyRepositoryBuilder().Build();
            return new RegisterUserUseCase(readOnlyRepoBuilder.Build(), writeOnlyRepo, unitOfWork, mapper, encripter);
        }


    }
}
