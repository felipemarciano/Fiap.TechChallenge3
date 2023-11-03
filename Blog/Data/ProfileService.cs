using System.Net.Http.Headers;

namespace Blog.Data
{
    public class ProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProfileModel?> Get()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = await _httpClient.GetAsync("api/profile");

            if (request.IsSuccessStatusCode)
            {
                var profileModel = await request.Content.ReadFromJsonAsync<ProfileModel>();

                return profileModel;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreateUpdatePost(string userName, string biography, string pictureUri, string gender)
        {
            var result = await _httpClient.PostAsJsonAsync("api/profile", new { UserName = userName, Biography = biography, PictureUri = pictureUri, Gender = gender });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePictureUri(string pictureUri)
        {
            var result = await _httpClient.PutAsJsonAsync("api/profile", new { PictureUri = pictureUri });

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return result.IsSuccessStatusCode;
        }
    }
}