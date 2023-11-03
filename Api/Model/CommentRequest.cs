namespace Api.Model
{
    public class CommentRequest
    {
        public Guid BlogPostId { get; set; }
        public string? Text { get; set; }
    }
}
