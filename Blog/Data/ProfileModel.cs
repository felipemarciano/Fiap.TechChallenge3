namespace Blog.Data
{
    public class ProfileModel
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Biography { get; set; }
        public string? PictureUri { get; set; }
        public string? Gender { get; set; }
    }
}
