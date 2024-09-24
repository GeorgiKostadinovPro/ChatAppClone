namespace ChatAppClone.Core
{
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Models.ViewModels.Images;
    using ChatAppClone.Models.ViewModels.Messages;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
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
                CreatedOn = DateTime.UtcNow.ToLocalTime()
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

        public async Task<ICollection<MessageViewModel>> GetByChatId(Guid chatId)
        {
            return await this.repository.AllReadonly<Message>()
                .Include(m => m.Images)
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    CreatorId = m.CreatorId,
                    Content = m.Content,
                    IsSeen = m.IsSeen,
                    CreatedOn = DateHelper.TimeAgo(m.CreatedOn),
                    MessageImages = m.Images.Select(mi => new ImageViewModel
                    {
                        Id = mi.Id,
                        Url = mi.Url
                    })
                })
                .ToArrayAsync();
        }
    }
}
