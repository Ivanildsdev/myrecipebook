using AutoMapper;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.MyExceptionBase;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserReadOnlyRepository _readOnlyUserRepository;
        private readonly IUserWriteOnlyRepository _writeOnlyUserRepository;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;
        private readonly IUnitOfWork _unitOfWork;


        public RegisterUserUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserWriteOnlyRepository userWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            PasswordEncripter passwordEncripter = null
            )
        {
            _readOnlyUserRepository = userReadOnlyRepository;
            _writeOnlyUserRepository = userWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter ?? new PasswordEncripter();
        }

        public async Task<ResponseRegisterUserJson> Execute(
            RequestRegisterUserJson request)
        {
            Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _passwordEncripter.Encript(request.Password);

            await _writeOnlyUserRepository.AddAsync(user);

            await _unitOfWork.CommitAsync();

            return new ResponseRegisterUserJson { Name = "" };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailExist = await _readOnlyUserRepository.ExistActiveUserWithEmail(request.Email);
            if (emailExist)
            {
                result.Errors.Add(new FluentValidation
                    .Results
                    .ValidationFailure(nameof(request.Email),
                                       ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var erroMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                throw new ErrorOnValidationException(erroMessages);
            }
        }
    }
}
