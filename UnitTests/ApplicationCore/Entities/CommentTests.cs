using ApplicationCore.Entities;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTests.ApplicationCore.Entities
{
    public class CommentTests
    {

        [Fact]
        public void UpdateText_UpdatesText()
        {
            // Arrange
            var comment = new Comment(Guid.NewGuid(), "Test", Guid.NewGuid());
            var newText = "New Text";

            // Act
            comment.UpdateText(newText);

            // Assert
            Assert.Equal(newText, comment.Text);
        }

        [Fact]
        public void AssociateWithProfile_AssociatesWithProfile()
        {
            // Arrange
            var comment = new Comment(Guid.NewGuid(), "Test", Guid.NewGuid());
            var newProfileId = Guid.NewGuid();

            // Act
            comment.AssociateWithProfile(newProfileId);

            // Assert
            Assert.Equal(newProfileId, comment.ProfileId);
        }

        [Fact]
        public void Constructor_SetsProfileId()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var comment = new Comment(profileId, text, blogPostId);

            // Assert
            Assert.Equal(profileId, comment.ProfileId);
        }

        [Fact]
        public void Constructor_SetsText()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var comment = new Comment(profileId, text, blogPostId);

            // Assert
            Assert.Equal(text, comment.Text);
        }

        [Fact]
        public void Constructor_SetsBlogPostId()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var comment = new Comment(profileId, text, blogPostId);

            // Assert
            Assert.Equal(blogPostId, comment.BlogPostId);
        }

        [Fact]
        public void Constructor_SetsDateCreate()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var comment = new Comment(profileId, text, blogPostId);

            // Assert
            Assert.NotEqual(default, comment.DateCreate);
        }

        [Fact]
        public void Constructor_SetsId()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var comment = new Comment(profileId, text, blogPostId);

            // Assert
            Assert.NotEqual(Guid.Empty, comment.Id);
        }

        [Fact]
        public void Constructor_ThrowsGivenEmptyProfileId()
        {
            // Arrange
            var profileId = Guid.Empty;
            var text = "Test";
            var blogPostId = Guid.NewGuid();

            // Act
            var exception = Record.Exception(() => new Comment(profileId, text, blogPostId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_ThrowsGivenEmptyText()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "";
            var blogPostId = Guid.NewGuid();

            // Act
            var exception = Record.Exception(() => new Comment(profileId, text, blogPostId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_ThrowsGivenEmptyBlogPostId()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = "Test";
            var blogPostId = Guid.Empty;

            // Act
            var exception = Record.Exception(() => new Comment(profileId, text, blogPostId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_ThrowsGivenNullText()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var text = (string?)null;
            var blogPostId = Guid.NewGuid();

            // Act
            var exception = Record.Exception(() => new Comment(profileId, text!, blogPostId));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void UpdateText_ThrowsGivenEmptyText()
        {
            // Arrange
            var comment = new Comment(Guid.NewGuid(), "Test", Guid.NewGuid());
            var newText = "";

            // Act
            var exception = Record.Exception(() => comment.UpdateText(newText));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void UpdateText_ThrowsGivenNullText()
        {
            // Arrange
            var comment = new Comment(Guid.NewGuid(), "Test", Guid.NewGuid());
            var newText = (string?)null;

            // Act
            var exception = Record.Exception(() => comment.UpdateText(newText!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
