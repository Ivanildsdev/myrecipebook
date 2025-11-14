using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //assert
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            //assert
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = "";

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And
                .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            //assert
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "";

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And
                .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            //assert
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "trd";

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And
                .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_Password_Invalid(int passwordLength)
        {
            //assert
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And
                .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
        }
    }
}
