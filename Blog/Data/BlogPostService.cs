using System.Net.Http.Headers;

namespace Blog.Data
{
    public class BlogPostService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogPostService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BlogPostModel>?> Get()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = await _httpClient.GetAsync("api/blogpost");

            if (request.IsSuccessStatusCode)
            {
                var blogPostModel = await request.Content.ReadFromJsonAsync<List<BlogPostModel>>();

                return blogPostModel;
            }
            else
            {
                return null;
            }
        }
    }
}