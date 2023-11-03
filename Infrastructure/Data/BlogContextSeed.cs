using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BlogContextSeed
    {
        public static void Seed(BlogContext blogContext)
        {
            if (blogContext.Database.IsNpgsql())
            {
                blogContext.Database.Migrate();
            }



        }
    }
}
