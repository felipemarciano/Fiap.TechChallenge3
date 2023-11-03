using ApplicationCore.Aggregates;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public sealed class ProfileApplicationUserIdSpecification : Specification<Profile>, ISingleResultSpecification<Profile>
    {
        public ProfileApplicationUserIdSpecification(Guid applicationUserId)
        {
            Query.
                Where(u => u.ApplicationUserId == applicationUserId);
        }
    }
}
