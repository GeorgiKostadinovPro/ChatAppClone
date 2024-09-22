namespace ChatAppClone.Core
{
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Models.ViewModels.Messages;
    using System.Threading.Tasks;

    public class MessageService : IMessageService
    {
        private readonly IRepository repository;
        public MessageService(IRepository _repository)
        {
            this.repository = _repository;
        }

        public async Task<MessageViewModel> CreateAsync(Guid chatId, string userId, string content)
        {
            Message message = new Message
            {
                ChatId = chatId,
                CreatorId = userId,
                Content = content,
                CreatedOn = DateTime.UtcNow
            };

            await this.repository.AddAsync(message);
            await this.repository.SaveChangesAsync();

            MessageViewModel model = new MessageViewModel
            {
                Id = message.Id,
                CreatorId = userId,
                Content = content,
                CreatedOn = DateHelper.TimeAgo(message.CreatedOn)
            };

            return model;
        }
    }
}
