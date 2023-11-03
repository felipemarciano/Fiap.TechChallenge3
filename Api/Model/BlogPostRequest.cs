namespace Api.Model
{
    public class BlogPostRequest
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Author { get; set; }
    }
}
