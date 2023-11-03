using ApplicationCore.Aggregates;
using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Moq;
using Xunit;

namespace UnitTests.ApplicationCore.Services
{
    public class PublicationServiceTests
    {
        private readonly Mock<IRepository<BlogPost>> _blogPostRepository;
        private readonly Mock<IRepository<Profile>> _profileRepository;
        private readonly PublicationService _publicationService;

        public PublicationServiceTests()
        {
            _blogPostRepository = new Mock<IRepository<BlogPost>>();
            _profileRepository = new Mock<IRepository<Profile>>();
            _publicationService = new PublicationService(_blogPostRepository.Object, _profileRepository.Object);
        }

        [Fact]
        public async Task PublishBlogPost_AddsBlogPostToRepository()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var title = "Test";
            var content = "Test";
            var author = "Test";

            // Act
            await _publicationService.PublishBlogPost(profileId, title, content, author);

            // Assert
            _blogPostRepository.Verify(x => x.AddAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddCommentToBlogPost_AddsCommentToBlogPost()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var comment = new Comment(Guid.NewGuid(), "Test", blogPost.Id);          
            var profile = new Profile(Guid.NewGuid(), "Test", "Test", "Test", EGender.Male);
            _blogPostRepository.Setup(x => x.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>())).ReturnsAsync(blogPost);
            _profileRepository.Setup(x => x.GetByIdAsync(comment.ProfileId, It.IsAny<CancellationToken>())).ReturnsAsync(profile);

            // Act
            await _publicationService.AddCommentToBlogPost(comment);

            // Assert
            Assert.Equal(comment, blogPost.Comments.Single());
        }

        [Fact]
        public async Task AddCommentToBlogPost_ThrowsInvalidOperationException_WhenBlogPostNotFound()
        {
            // Arrange
            var blogPostId = Guid.NewGuid();
            var comment = new Comment(Guid.NewGuid(), "Test", blogPostId);
            _blogPostRepository.Setup(x => x.GetByIdAsync(blogPostId, It.IsAny<CancellationToken>())).ReturnsAsync((BlogPost)null!);

            // Act
            var exception = await Record.ExceptionAsync(() => _publicationService.AddCommentToBlogPost(comment));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public async Task AddCommentToBlogPost_ThrowsInvalidOperationException_WhenProfileNotFound()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var comment = new Comment(Guid.NewGuid(), "Test", blogPost.Id);

            _blogPostRepository.Setup(x => x.GetByIdAsync(blogPost.Id, It.IsAny<CancellationToken>())).ReturnsAsync(blogPost);
            _profileRepository.Setup(x => x.GetByIdAsync(comment.ProfileId, It.IsAny<CancellationToken>())).ReturnsAsync((Profile)null!);

            // Act
            var exception = await Record.ExceptionAsync(() => _publicationService.AddCommentToBlogPost(comment));

            // Assert
            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
