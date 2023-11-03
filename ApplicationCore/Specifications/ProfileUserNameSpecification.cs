using ApplicationCore.Aggregates;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public sealed class ProfileUserNameSpecification : Specification<Profile>, ISingleResultSpecification<Profile>
    {
        public ProfileUserNameSpecification(string userName, Guid applicationUserId)
        {
            Query.
                Where(u => u.UserName == userName && u.ApplicationUserId != applicationUserId);
        }
        public ProfileUserNameSpecification(Guid applicationUserId)
        {
            Query.
                Where(u => u.ApplicationUserId == applicationUserId);
        }
    }
}
