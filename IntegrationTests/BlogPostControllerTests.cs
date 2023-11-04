using Api.Model;
using IntegrationTests.Helpers;
using IntegrationTests.Setup;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class BlogPostControllerTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;

        public BlogPostControllerTests(ApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfBlogPosts()
        {
            // Arrange
            var client = _factory.CreateClient();
            var helper = new ApiHelpers(client);
            await helper.AuthenticateAdminAsync();
            await helper.CreateListBlogPostAsync(5);

            // Act
            var response = await client.GetAsync("/api/blogpost");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithBlogPost()
        {
            // Arrange
            var client = _factory.CreateClient();
            var helper = new ApiHelpers(client);
            await helper.AuthenticateAdminAsync();
            await helper.CreateListBlogPostAsync(5);

            var responseBlob = await client.GetAsync("/api/blogpost");

            var listBlob = await responseBlob.Content.ReadFromJsonAsync<IEnumerable<BlogPostResponse>>();

            // Act            
            var response = await client.GetAsync($"/api/blogpost/{listBlob!.First().Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WhenBlogPostIsPosted()
        {
            // Arrange
            var client = _factory.CreateClient();
            var helper = new ApiHelpers(client);
            await helper.AuthenticateAdminAsync();

            var blogPostRequest = new BlogPostRequest
            {
                Title = "Test",
                Content = "Test",
                Author = "Test"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/blogpost", blogPostRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.AlreadyReported, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorizedResult_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var client = _factory.CreateClient();
            var blogPostRequest = new BlogPostRequest
            {
                Title = "Test",
                Content = "Test",
                Author = "Test"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/blogpost", blogPostRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsBadRequestResult_WhenPublicationFails()
        {
            // Arrange
            var client = _factory.CreateClient();
            var helper = new ApiHelpers(client);
            await helper.AuthenticateAdminAsync();

            var blogPostRequest = new BlogPostRequest
            {
                Title = null!,
                Content = "Test",
                Author = "Test"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/blogpost", blogPostRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}