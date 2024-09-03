namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;

    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task SetUserProfilePictureAsync(string userId, string url, string publicId);
    }
}
