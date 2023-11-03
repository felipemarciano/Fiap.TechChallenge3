using ApplicationCore.Aggregates;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> _profileRepository;

        public ProfileService(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task CreateUpdateProfileAsync(Profile profile)
        {
            //Check existing username
            var profileUserNameExistingSpecification = new ProfileUserNameSpecification(profile.UserName, profile.ApplicationUserId);
            var profileDuplicateCount = await _profileRepository.CountAsync(profileUserNameExistingSpecification);
            if (profileDuplicateCount > 0)
            {
                throw new DuplicateException($"User already exists {profile.UserName}");
            }

            var profileUserNameSpecification = new ProfileUserNameSpecification(profile.ApplicationUserId);
            var profileExisting = await _profileRepository.FirstOrDefaultAsync(profileUserNameSpecification);

            if (profileExisting != null)
            {
                profileExisting.ChangeProfile(profile.UserName, profile.Biography, profile.Gender);

                if (!string.IsNullOrEmpty(profile.PictureUri))
                    profileExisting.ChangePictureUri(profile.PictureUri);

                await _profileRepository.UpdateAsync(profileExisting);
            }
            else
            {
                await _profileRepository.AddAsync(profile);
            }
        }

        public async Task UpdatePictureUriAsync(Guid applicationUserId, string pictureUri)
        {
            var profileUserNameSpecification = new ProfileUserNameSpecification(applicationUserId);
            var profileExisting = await _profileRepository.FirstOrDefaultAsync(profileUserNameSpecification);

            if (profileExisting == null)
            {
                throw new ArgumentNullException("Profile not found!");
            }
            else
            {
                profileExisting.ChangePictureUri(pictureUri);
                await _profileRepository.UpdateAsync(profileExisting);
            }
        }
    }
} 
