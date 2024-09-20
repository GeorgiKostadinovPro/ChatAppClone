namespace ChatAppClone.Core
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    using ChatAppClone.Common.Messages;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using System.Collections.Generic;
    using ChatAppClone.Models.ViewModels.Users;

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
                throw new InvalidOperationException(UserMessages.AlreadyExists);
            }

            return user;
        }

        public async Task<IEnumerable<UserCardViewModel>> GetUsersAsync(string userId, ExploreUsersQueryModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetUsersCountAsync()
        {
            throw new NotImplementedException();
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

            if (user.ProfilePicturePublicId == null)
            {
                throw new InvalidOperationException(UserMessages.NOTExistingPicture);
            }

            user.ProfilePictureUrl = null;
            user.ProfilePicturePublicId= null;

            await this.userManager.UpdateAsync(user);
        }
    }
}
