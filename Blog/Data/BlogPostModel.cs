namespace Blog.Data
{
    public class BlogPostModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public List<CommentModel>? Comments { get; set; }
    }
}
