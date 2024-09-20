namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Users;

    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(string userId);

        Task<IEnumerable<UserCardViewModel>> GetUsersAsync(string userId, ExploreUsersQueryModel model);

        Task<int> GetUsersCountAsync();

        Task SetProfilePictureAsync(string userId, string url, string publicId);

        Task DeleteProfilePictureAsync(string userId);
    }
}
