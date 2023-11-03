using ApplicationCore.Aggregates;
using Ardalis.GuardClauses;
using System.Runtime.Intrinsics.Arm;
using static System.Net.Mime.MediaTypeNames;

namespace ApplicationCore.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public Guid BlogPostId { get; private set; }
        public BlogPost? BlogPost { get; private set; }
        public DateTime DateCreate { get; private set; }
        public Guid ProfileId { get; private set; }
        public Profile? Profile { get; private set; }

        public Comment(Guid profileId, string text, Guid blogPostId)
        {
            Guard.Against.NullOrEmpty(blogPostId, nameof(blogPostId));
            Guard.Against.NullOrEmpty(profileId, nameof(profileId));
            Guard.Against.NullOrEmpty(text, nameof(text));

            Id = Guid.NewGuid();
            ProfileId = profileId;
            Text = text;
            BlogPostId = blogPostId;
            DateCreate = DateTime.Now;           
        }

        public void UpdateText(string newText)
        {
            Guard.Against.NullOrEmpty(newText, nameof(newText));

            Text = newText;
        }

        public void AssociateWithProfile(Guid profileId)
        {
            ProfileId = profileId;
        }
    }
}
