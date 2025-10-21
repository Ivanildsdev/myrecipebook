using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";
        private readonly string _name;
        private readonly string _email;
        private readonly Guid _userIdentifier;
        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            //arrange
            //(var user, var password) = CommonTestUtilities.Entities.UserBuilder.Build();
            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            //arrange
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            //act
            var response = await DoGet(METHOD, token: token);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData
                .RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .NotBeNullOrWhiteSpace()
                .And
                .Be(_name);

            responseData
                .RootElement
                .GetProperty("email")
                .GetString()
                .Should()
                .NotBeNullOrWhiteSpace()
                .And
                .Be(_email);

        }

    }
}
