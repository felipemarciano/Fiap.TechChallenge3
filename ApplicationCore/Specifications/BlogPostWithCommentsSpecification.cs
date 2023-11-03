using ApplicationCore.Aggregates;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class BlogPostWithCommentsSpecification : Specification<BlogPost>, ISingleResultSpecification<BlogPost>
    {
        public BlogPostWithCommentsSpecification(Guid postId)
        {
            Query
                .Where(post => post.Id == postId)
                .Include(post => post.Comments);
        }


        public BlogPostWithCommentsSpecification()
        {
            Query
                .Include(post => post.Comments);
        }
    }
}
