namespace Api.Model
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid ProfileId { get; set; }
        public string? Text { get; set; }
    }
}
