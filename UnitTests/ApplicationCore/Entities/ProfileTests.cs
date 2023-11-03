using Xunit;
using ApplicationCore.Aggregates;
using ApplicationCore.Entities;
using ApplicationCore.Constants;

namespace UnitTests.ApplicationCore.Entities
{
    public class ProfileTests
    {
        [Fact]
        public void Shold_ChangePictureUri()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "test", "test", "test", EGender.Uninformed);
            var newPictureUri = "test2";
            // Act
            profile.ChangePictureUri(newPictureUri);

            // Assert
            Assert.Equal(profile.PictureUri, newPictureUri);
        }

        [Fact]
        public void Shold_DeletePictureUri()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "test", "test", "test", EGender.Uninformed);
            // Act
            profile.DeletePictureUri();

            // Assert
            Assert.Equal(null!, profile.PictureUri);
        }

        [Fact]
        public void Shold_CreateProfile()
        {
            // Arrange
            var profile = new Profile(Guid.NewGuid(), "test", "test", "test", EGender.Uninformed);

            // Assert
            Assert.Equal(profile.ApplicationUserId, profile.Id );
        }
    }
}
