using ApplicationCore.Aggregates;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public class ProfileSpecification : Specification<Profile>, ISingleResultSpecification<Profile>
    {
        public ProfileSpecification(Guid applicationUserId)
        {
            Query.
                Where(u => u.ApplicationUserId == applicationUserId);
        }
    }
}
