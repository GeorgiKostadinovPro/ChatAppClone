namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Chats;
    using ChatAppClone.Models.ViewModels.Users;

    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(string userId);

        Task<ICollection<UserCardViewModel>> GetAsync(string userId, ExploreUsersQueryModel model);

        Task<int> GetCountAsync(string? searchTerm = null);

        Task<ICollection<ParticipantViewModel>> GetByChatAsync(Guid chatId);

        Task SetProfilePictureAsync(string userId, string url, string publicId);

        Task DeleteProfilePictureAsync(string userId);
    }
}
