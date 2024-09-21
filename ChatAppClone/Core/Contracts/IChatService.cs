namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;

    public interface IChatService
    {
        Task<Chat?> GetChatByIdAsync(Guid chatId);

        Task<IEnumerable<Chat>> GetChatsByUserAsync(string userId);

        Task<Chat> CreateChatAsync(string userAId, string userBId);
    }
}
