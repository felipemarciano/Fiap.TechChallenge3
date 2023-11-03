using ApplicationCore.Aggregates;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IPublicationService
    {
        Task PublishBlogPost(Guid profileId, string title, string content, string author);
        Task AddCommentToBlogPost(Comment comment);
    }
}
