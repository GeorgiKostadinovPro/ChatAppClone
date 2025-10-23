namespace ChatAppClone.Core
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Models.ViewModels.Chats;
    using ChatAppClone.Models.ViewModels.Users;
    
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Common.Messages;

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
                throw new InvalidOperationException(UserMessages.DoesNotExist);
            }

            user.ProfilePictureUrl = user.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl;

            return user;
        }

        public async Task<ICollection<UserCardViewModel>> GetAsync(string userId, ExploreUsersQueryModel model)
        {
            IQueryable<ApplicationUser> usersQuery = this.repository
                .AllReadonly<ApplicationUser>()
                .Include(u => u.UsersChats)
                .Where(u => u.Id != userId);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                string wildCard = $"%{model.SearchTerm.ToLower()}%";

                usersQuery = usersQuery
                    .Where(u => EF.Functions.Like(u.UserName, wildCard)
                    || EF.Functions.Like(u.Email, wildCard));
            }

            ICollection<UserCardViewModel> users
                = await usersQuery
                             .Skip((model.CurrentPage - 1) * model.UsersPerPage)
                             .Take(model.UsersPerPage)
                             .Select(u => new UserCardViewModel
                             {
                                 Id = u.Id,
                                 Username = u.UserName!,
                                 ProfilePictureUrl = u.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl,
                                 CreatedOn = DateHelper.GetDate(u.CreatedOn),
                                 ChatsCount = u.UsersChats.Count
                             })
                             .ToArrayAsync();

            return users;
        }

        public async Task<int> GetCountAsync(string? searchTerm = null)
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

        public async Task<ICollection<ParticipantViewModel>> GetByChatAsync(Guid chatId)
        {
            return await this.repository.AllReadonly<UserChat>()
                .Include(uc => uc.User)
                .Where(uc => uc.ChatId == chatId)
                .Select(uc => new ParticipantViewModel
                {
                    Id = uc.UserId,
                    ProfilePictureUrl = uc.User.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl
                }).ToArrayAsync();
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
