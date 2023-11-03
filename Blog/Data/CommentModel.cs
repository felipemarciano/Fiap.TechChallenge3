namespace Blog.Data
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid ProfileId { get; set; }
        public string? Text { get; set; }
    }
}
