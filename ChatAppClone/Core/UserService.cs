namespace ChatAppClone.Core
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    using ChatAppClone.Common.Messages;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using System.Collections.Generic;
    using ChatAppClone.Models.ViewModels.Users;
    using ChatAppClone.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using ChatAppClone.Common.Constants;

    public class UserService : IUserService
    {
        private readonly IRepository repository;

        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IRepository _repository, UserManager<ApplicationUser> _userManager)
        {
            this.repository = _repository;  
            this.userManager = _userManager;
        }
       
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException(UserMessages.AlreadyExists);
            }

            return user;
        }

        public async Task<IEnumerable<UserCardViewModel>> GetUsersAsync(string userId, ExploreUsersQueryModel model)
        {
            IQueryable<ApplicationUser> usersQuery = this.repository
                .AllReadonly<ApplicationUser>()
                .Where(u => u.Id != userId)
                .Include(u => u.Followers);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                string wildCard = $"%{model.SearchTerm.ToLower()}%";

                usersQuery = usersQuery
                    .Where(u => EF.Functions.Like(u.UserName, wildCard)
                    || EF.Functions.Like(u.Email, wildCard));
            }

            IEnumerable<UserCardViewModel> users
                = await usersQuery
                             .Skip((model.CurrentPage - 1) * model.UsersPerPage)
                             .Take(model.UsersPerPage)
                             .Select(u => new UserCardViewModel
                             {
                                 Id = u.Id,
                                 Username = u.UserName!,
                                 ProfilePictureUrl = u.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl,
                                 CreatedOn = u.CreatedOn.ToString("dd MMM yyyy"),
                                 FollowersCount = u.Followers.Count(),
                                 IsFollowed = u.Followers.Any(f => f.FollowerId == userId),
                             })
                             .ToArrayAsync();

            return users;
        }

        public async Task<int> GetUsersCountAsync(string? searchTerm = null)
        {
            IQueryable<ApplicationUser> usersQuery = this.repository.AllReadonly<ApplicationUser>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string wildCard = $"%{searchTerm.ToLower()}%";

                usersQuery = usersQuery
                    .Where(u => EF.Functions.Like(u.UserName, wildCard)
                    || EF.Functions.Like(u.Email, wildCard));
            }

            return await usersQuery.CountAsync();
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
            user.ProfilePicturePublicId = null;

            await this.userManager.UpdateAsync(user);
        }
    }
}
