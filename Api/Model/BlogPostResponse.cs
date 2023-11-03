namespace Api.Model
{
    public class BlogPostResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public List<CommentResponse>? Comments { get; set; }
    }
}
