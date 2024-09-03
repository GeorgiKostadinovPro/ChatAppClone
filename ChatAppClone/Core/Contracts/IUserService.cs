namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;

    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(string userId);

        Task SetProfilePictureAsync(string userId, string url, string publicId);

        Task DeleteProfilePictureAsync(string userId);
    }
}
