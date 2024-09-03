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
       
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User with such id does not exist.");
            }

            return user;
        }

        public async Task SetProfilePictureAsync(string userId, string url, string publicId)
        {
            ApplicationUser user = await this.GetByIdAsync(userId);

            user.ProfilePictureUrl = url;
            user.ProfilePicturePublicId = publicId;

            await this.userManager.UpdateAsync(user);
        } 
        
        public async Task DeleteProfilePictureAsync(string userId)
        {
            ApplicationUser user = await this.GetByIdAsync(userId);

            user.ProfilePictureUrl = null;
            user.ProfilePicturePublicId= null;

            await this.userManager.UpdateAsync(user);
        }
    }
}
