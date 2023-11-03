using Api.Model;
using ApplicationCore.Constants;
using System.Net.Http.Json;

namespace IntegrationTests.Helpers
{
    public class ApiHelpers
    {

        private readonly HttpClient _httpClient;

        public ApiHelpers(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AuthenticateAdminAsync()
        {
            var username = "admin@microsoft.com";
            var password = AuthorizationConstants.DEFAULT_PASSWORD;

            var result = await _httpClient.PostAsJsonAsync("api/account/login", new { username, password });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            var jwtToken = await result.Content.ReadFromJsonAsync<RToken>() ?? new();

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken.AccessToken}");
            var resultProfile = await _httpClient.PostAsJsonAsync("api/profile", new ProfileRequest
            {
                UserName = username
            });

            if (!resultProfile.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return jwtToken.AccessToken ?? "";
        }

        public async Task<string> AuthenticateUserAsync()
        {
            var username = "demouser@microsoft.com";
            var password = AuthorizationConstants.DEFAULT_PASSWORD;

            var result = await _httpClient.PostAsJsonAsync("api/account/login", new { username, password });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            var jwtToken = await result.Content.ReadFromJsonAsync<RToken>() ?? new();

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken.AccessToken}");
            var resultProfile = await _httpClient.PostAsJsonAsync("api/profile", new ProfileRequest
            {
                UserName = username,
            });

            if (!resultProfile.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return jwtToken.AccessToken ?? "";
        }

        public async Task CreateListBlogPostAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var result = await _httpClient.PostAsJsonAsync("api/blogpost", new BlogPostRequest
                {
                    Author = $"Test-{i}",
                    Content = $"Test-{i}",
                    Title = $"Test-{i}"
                });

                if (!result.IsSuccessStatusCode)
                {
                    throw new Exception(await result.Content.ReadAsStringAsync());
                }
            }
        }



        private class RToken
        {
            public string? AccessToken { get; set; }
        }
    }
}
