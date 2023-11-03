using ApplicationCore.Aggregates;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Profile> _profileRepository;

        public PublicationService(IRepository<BlogPost> blogPostRepository, IRepository<Profile> profileRepository)
        {
            _blogPostRepository = blogPostRepository;
            _profileRepository = profileRepository;
        }

        public async Task PublishBlogPost(Guid profileId, string title, string content, string author)
        {
            var blogPost = new BlogPost(profileId, title, content, author);

            await _blogPostRepository.AddAsync(blogPost);
        }

        public async Task AddCommentToBlogPost(Comment comment)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(comment.BlogPostId);

            if (blogPost == null)
            {
                throw new InvalidOperationException("Blog post not found.");
            }

            var profile = await _profileRepository.GetByIdAsync(comment.ProfileId);

            if (profile is null)
            {
                throw new InvalidOperationException("Profile not found, please fill in the profile");
            }

            blogPost.AddComment(comment);
            await _blogPostRepository.UpdateAsync(blogPost);
        }
    }
}
