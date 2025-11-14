using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Register
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //assert
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            //assert
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
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
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
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
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "trd";

            //act
            var result = validator.Validate(request);

            //assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And
                .Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        }
    }
}
