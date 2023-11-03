using Api.Controllers;
using Api.Model;
using ApplicationCore.Aggregates;
using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace UnitTests.Api.Controllers
{
    public class BlogPostControllerTests
    {
        private readonly Mock<ILogger<BlogPostController>> _mockLogger;
        private readonly Mock<IReadRepository<BlogPost>> _mockBlogPostRepository;
        private readonly Mock<IPublicationService> _mockPublicationService;
        private readonly Mock<IReadRepository<Profile>> _mockProfileRepository;
        private readonly BlogPostController _controller;

        public BlogPostControllerTests()
        {
            _mockLogger = new Mock<ILogger<BlogPostController>>();
            _mockBlogPostRepository = new Mock<IReadRepository<BlogPost>>();
            _mockPublicationService = new Mock<IPublicationService>();
            _mockProfileRepository = new Mock<IReadRepository<Profile>>();

            _controller = new BlogPostController(
            _mockLogger.Object,
            _mockBlogPostRepository.Object,
            _mockPublicationService.Object,
            _mockProfileRepository.Object);

            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userId),
            }));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfBlogPosts()
        {
            // Arrange
            var listBlogPosts = new List<BlogPost>
            {
                new BlogPost(Guid.NewGuid(), "Test", "Test", "Test"),
                new BlogPost(Guid.NewGuid(), "Test2", "Test", "Test"),
                new BlogPost(Guid.NewGuid(), "Test3", "Test", "Test"),
                new BlogPost(Guid.NewGuid(), "Test4", "Test", "Test"),
                new BlogPost(Guid.NewGuid(), "Test5", "Test", "Test")
            };

            _mockBlogPostRepository.Setup(repo => repo.ListAsync(It.IsAny<BlogPostWithCommentsSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listBlogPosts);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithBlogPost()
        {

            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var comment = new Comment(Guid.NewGuid(), "Test", blogPost.Id);

            var blogPostId = Guid.NewGuid();
            _mockBlogPostRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BlogPostWithCommentsSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(blogPost);

            // Act
            var result = await _controller.GetById(blogPostId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WhenPublicationSucceeds()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "Test", "Test", "Test", EGender.Male);

            var blogPostRequest = new BlogPostRequest
            {
                Author = "Test",
                Content = "Test",
                Title = "Test"
            };
            _mockProfileRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ProfileSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);

            // Act
            var result = await _controller.Post(blogPostRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequestResult_WhenPublicationFails()
        {
            // Arrange
            var blogPostRequest = new BlogPostRequest
            {
                Author = "Test",
                Content = "Test",
                Title = "Test"
            };

            var profile = new Profile(Guid.NewGuid(), "Test", "Test", "Test", EGender.Male);

            _mockProfileRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ProfileSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(profile);

            _mockPublicationService.Setup(service => service.PublishBlogPost(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                    .ThrowsAsync(new Exception("Publication failed"));

            // Act
            var result = await _controller.Post(blogPostRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorizedResult_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var blogPostRequest = new BlogPostRequest
            {
                Author = "Test",
                Content = "Test",
                Title = "Test"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.Post(blogPostRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }

}
