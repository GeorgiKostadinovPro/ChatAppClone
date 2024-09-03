namespace ChatAppClone.Core
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> _userManager)
        {
            this.userManager = _userManager;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User with such id does not exist.");
            }

            return user;
        }

        public async Task SetUserProfilePictureAsync(string userId, string url, string publicId)
        {
            ApplicationUser user = await this.GetUserByIdAsync(userId);

            user.ProfilePictureUrl = url;
            user.ProfilePicturePublicId = publicId;

            await userManager.UpdateAsync(user);
        }
    }
}
