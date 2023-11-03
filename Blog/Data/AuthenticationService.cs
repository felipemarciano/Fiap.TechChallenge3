namespace Blog.Data
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/login", new { username, password });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            var jwtToken = await result.Content.ReadFromJsonAsync<RToken>() ?? new();
            return jwtToken.AccessToken ?? "";
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register", new { Email = username, Password = password });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return result.IsSuccessStatusCode;
        }

        private class RToken
        {
            public string? AccessToken { get; set; }
        }
    }
}
