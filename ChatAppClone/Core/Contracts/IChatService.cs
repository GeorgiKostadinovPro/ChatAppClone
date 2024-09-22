namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Chats;

    public interface IChatService
    {
        Task<ChatViewModel> GetByIdAsync(Guid chatId);

        Task<ICollection<ChatViewModel>> GetByUserAsync(string userId);

        Task<Chat> CreateAsync(string userAId, string userBId);

        Task<bool> IsValidAsync(Guid chatId);
    }
}
