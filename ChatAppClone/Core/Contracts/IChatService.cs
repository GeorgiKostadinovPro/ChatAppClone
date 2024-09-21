namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Chats;

    public interface IChatService
    {
        Task<ChatViewModel> GetChatByIdAsync(Guid chatId);

        Task<ICollection<ChatViewModel>> GetChatsByUserAsync(string userId);

        Task<Chat> CreateChatAsync(string userAId, string userBId);
    }
}
