using FluentValidation.Results;
using MyRecipeBook.Application.UseCases.User.ChgangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Criptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.MyExceptionBase;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Application.UseCases.User.Update
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _updateUserRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncripter _passwordEncripter;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository updateUserRepository,
            IPasswordEncripter passwordEncripter,
            IUnitOfWork unitOfWork)
        {
            _updateUserRepository = updateUserRepository;
            _loggedUser = loggedUser;
            _passwordEncripter = passwordEncripter;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser);

            var user = await _updateUserRepository.GetById(loggedUser.Id);

            user.Password = _passwordEncripter.Encript(request.NewPassword);

            _updateUserRepository.Update(user);

            await _unitOfWork.CommitAsync();
        }

        private async Task Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
        {
            var result = new ChangePasswordValidator().Validate(request);

            var currentPasswordEncripted = _passwordEncripter.Encript(request.Password);
            if (!currentPasswordEncripted.Equals(loggedUser.Password))
            {
                result.Errors.Add(new ValidationFailure("", ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if (!result!.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
