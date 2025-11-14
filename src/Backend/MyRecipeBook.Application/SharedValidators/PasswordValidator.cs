using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.SharedValidators
{
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {
        const int MAX_LENGTH = 6;
        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);
                return false;
            }

            if (value.Length < MAX_LENGTH)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.INVALID_PASSWORD);
                return false;
            }

            return true;
        }

        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
    }
}
