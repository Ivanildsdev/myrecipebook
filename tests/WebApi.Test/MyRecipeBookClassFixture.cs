using System.Net.Http.Json;

namespace WebApi.Test
{
    public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public MyRecipeBookClassFixture(CustomWebApplicationFactory factory) => _client = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
        {   SetCulture(culture);
            return await _client.PostAsJsonAsync(method, request);
        }
        
        private void SetCulture(string culture)
        {
            if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
                _client.DefaultRequestHeaders.Remove("Accept-Language");
            _client.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

    }
}
