namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Models.ViewModels.Messages;

    public interface IMessageService
    {
        Task<MessageViewModel> CreateAsync(Guid chatId, string userId, string content);
    }
}
