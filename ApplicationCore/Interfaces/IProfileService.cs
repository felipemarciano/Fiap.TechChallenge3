using ApplicationCore.Aggregates;

namespace ApplicationCore.Interfaces
{
    public interface IProfileService
    {
        Task CreateUpdateProfileAsync(Profile profile);
        Task UpdatePictureUriAsync(Guid applicationUserId, string pictureUri);
    }
}
