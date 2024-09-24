namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Chats;

    public interface IChatService
    {
        Task<Chat> CreateAsync(string userAId, string userBId);

        Task<ChatViewModel> GetByIdAsync(Guid chatId);

        Task<ICollection<ChatViewModel>> GetByUserAsync(string userId);

        Task<bool> IsValidAsync(Guid chatId);
    }
}
