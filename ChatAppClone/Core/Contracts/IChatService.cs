namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Models.ViewModels.Chats;

    public interface IChatService
    {
        Task<ChatViewModel> CreateAsync(string userAId, string userBId);

        Task<ChatViewModel> GetByIdAsync(Guid chatId);

        Task<ICollection<ChatViewModel>> GetByUserAsync(string userId);

        Task<bool> IsValidAsync(Guid chatId);

        Task<bool> CheckIfChatExists(string userAId, string userBId);

        Task<ChatViewModel> DeleteAsync(Guid chatId);
    }
}
