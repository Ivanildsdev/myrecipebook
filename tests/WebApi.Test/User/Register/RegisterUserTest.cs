using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : MyRecipeBookClassFixture
    {
        private readonly string method = "User";

        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            //arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            
            //act
            var response = await DoPost(method, request);
            
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
      
            var responseData = await JsonDocument.ParseAsync(responseBody);
          
            responseData
                .RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .NotBeNullOrWhiteSpace()
                .And
                .Be(request.Name);

            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
                .Should()
                .NotBeNullOrEmpty();

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string language)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(method, request, language);
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new System.Globalization.CultureInfo(language));
            
            errors.Should()
                .ContainSingle()
                .And
                .Contain(e => e.GetString().Equals(expectedMessage));
        }
    }
}
