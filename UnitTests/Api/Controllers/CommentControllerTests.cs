using Api.Controllers;
using Api.Model;
using ApplicationCore.Aggregates;
using ApplicationCore.Constants;
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
    public class CommentControllerTests
    {
        private readonly Mock<ILogger<CommentController>> _mockLogger;
        private readonly Mock<IPublicationService> _mockPublicationService;
        private readonly Mock<IReadRepository<Profile>> _mockProfileRepository;

        public CommentControllerTests()
        {
            _mockLogger = new Mock<ILogger<CommentController>>();
            _mockPublicationService = new Mock<IPublicationService>();
            _mockProfileRepository = new Mock<IReadRepository<Profile>>();
        }

        [Fact]
        public async Task Post_ReturnsOkResult_WhenCommentIsPosted()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "Test", "Test", "Test", EGender.Male);

            var controller = new CommentController(
                _mockLogger.Object,
                _mockPublicationService.Object,
                _mockProfileRepository.Object);

            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, userId),
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var commentRequest = new CommentRequest
            {
                BlogPostId = Guid.NewGuid(),
                Text = "Test"
            };

            typeof(Profile).GetProperty(nameof(profile.Id))?.SetValue(profile, Guid.NewGuid(), null);

            _mockProfileRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ProfileSpecification>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(profile);
            // Act
            var result = await controller.Post(commentRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorizedResult_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var controller = new CommentController(
                _mockLogger.Object,
                _mockPublicationService.Object,
                _mockProfileRepository.Object);

            // User is not set, simulating an unauthenticated request

            var commentRequest = new CommentRequest { /* ... */ };

            // Act
            var result = await controller.Post(commentRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsBadRequestResult_WhenExceptionIsThrown()
        {
            // Arrange
            var controller = new CommentController(
                _mockLogger.Object,
                _mockPublicationService.Object,
                _mockProfileRepository.Object);

            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, userId),
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var commentRequest = new CommentRequest { /* ... */ };
            _mockProfileRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<ProfileSpecification>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await controller.Post(commentRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Database error", badRequestResult!.Value);
        }
    }

}
