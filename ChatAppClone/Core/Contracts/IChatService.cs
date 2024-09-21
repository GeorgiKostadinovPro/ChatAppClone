namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;

    public interface IChatService
    {
        Task<Chat?> GetChatByIdAsync(Guid chatId);

        Task<ICollection<Chat>> GetChatsByUserAsync(string userId);

        Task<Chat> CreateChatAsync(string userAId, string userBId);
    }
}
