﻿using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator() 
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(50).WithMessage(ResourceMessagesException.NAME_LENGTH);

            RuleFor(x => x.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            RuleFor(x => x.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessagesException.PASSWORD_LENGTH);
            When(user => !string.IsNullOrEmpty(user.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
        }
    }
}
