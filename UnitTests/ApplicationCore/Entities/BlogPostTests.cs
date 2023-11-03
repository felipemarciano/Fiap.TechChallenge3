using Xunit;
using ApplicationCore.Aggregates;
using ApplicationCore.Entities;
using ApplicationCore.Constants;

namespace UnitTests.ApplicationCore.Entities
{
    public class BlogPostTests
    {
        [Fact]
        public void UpdateTitle_UpdatesTitle()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var newTitle = "New Title";

            // Act
            blogPost.UpdateTitle(newTitle);

            // Assert
            Assert.Equal(newTitle, blogPost.Title);
        }

        [Fact]
        public void UpdateContent_UpdatesContent()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var newContent = "New Content";

            // Act
            blogPost.UpdateContent(newContent);

            // Assert
            Assert.Equal(newContent, blogPost.Content);
        }

        [Fact]
        public void AssociateWithProfile_AssociatesWithProfile()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var newProfileId = Guid.NewGuid();

            // Act
            blogPost.AssociateWithProfile(newProfileId);

            // Assert
            Assert.Equal(newProfileId, blogPost.ProfileId);
        }

        [Fact]
        public void AddComment_AddsComment()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var comment = new Comment(Guid.NewGuid(), "Test", blogPost.Id);

            // Act
            blogPost.AddComment(comment);

            // Assert
            Assert.Equal(comment, blogPost.Comments.Single());
        }

        [Fact]
        public void RemoveComment_RemovesComment()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");
            var comment = new Comment(Guid.NewGuid(), "Test", blogPost.Id);
            blogPost.AddComment(comment);

            // Act
            blogPost.RemoveComment(comment);

            // Assert
            Assert.Empty(blogPost.Comments);
        }

        [Fact]
        public void CreateBlogPost_CreatesBlogPost()
        {
            // Arrange
            var blogPost = new BlogPost(Guid.NewGuid(), "Test", "Test", "Test");

            // Assert
            Assert.NotEqual(blogPost.Id, Guid.Empty);
        }

        [Fact]
        public void CreateBlogPost_ThrowsException_WhenProfileIdIsEmpty()
        {
            // Arrange
            var profileId = Guid.Empty;
            var title = "Test";
            var content = "Test";
            var author = "Test";

            // Act
            var exception = Record.Exception(() => new BlogPost(profileId, title, content, author));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void CreateBlogPost_ThrowsException_WhenContentIsEmpty()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var title = "Test";
            var content = string.Empty;
            var author = "Test";

            // Act
            var exception = Record.Exception(() => new BlogPost(profileId, title, content, author));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
