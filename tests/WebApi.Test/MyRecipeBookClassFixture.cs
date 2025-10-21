﻿using System.Net.Http.Headers;
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

        protected async Task<HttpResponseMessage> DoGet(string method, string token, string culture = "en")
        {
            SetCulture(culture);
            AuthorizeRequest(token);
            return await _client.GetAsync(method);
        }

        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void SetCulture(string culture)
        {
            if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
                _client.DefaultRequestHeaders.Remove("Accept-Language");
            _client.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

    }
}
