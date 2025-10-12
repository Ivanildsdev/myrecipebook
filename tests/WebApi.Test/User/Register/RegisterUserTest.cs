using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public RegisterUserTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            //arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            
            //act
            var response = await _client.PostAsJsonAsync("User", request);
            
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

        }
        
        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string language)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
               _client.DefaultRequestHeaders.Remove("Accept-Language");

            _client.DefaultRequestHeaders.Add("Accept-Language", language);

            var response = await _client.PostAsJsonAsync("User", request);

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
