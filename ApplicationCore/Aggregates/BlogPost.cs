using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.GuardClauses;

namespace ApplicationCore.Aggregates
{
    public class BlogPost : IAggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid ProfileId { get; private set; } 
        public Profile? Profile { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Author { get; private set; }
        public byte[] Timestamp { get; private set; }

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        public BlogPost(Guid profileId, string title, string content, string author)
        {
            Guard.Against.NullOrEmpty(profileId, nameof(profileId));
            Guard.Against.NullOrEmpty(content, nameof(content));
            Guard.Against.NullOrEmpty(author, nameof(author));

            Id = Guid.NewGuid();
            ProfileId = profileId;  
            Title = title;
            Content = content;
            Author = author;
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
        }

        public void AssociateWithProfile(Guid profileId)
        {
            ProfileId = profileId;
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
        }

        public void UpdateTitle(string newTitle)
        {
            Title = newTitle;
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
        }

        public void AddComment(Comment comment)
        {
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
            _comments.Add(comment);
        }

        public void RemoveComment(Comment comment)
        {
            Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
            _comments.Remove(comment);
        }
    }
}
