namespace ChatAppClone.Core
{
    using System;
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Models.ViewModels.Chats;
    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;
        private readonly IMessageService messageService;

        public ChatService(
            IRepository _repository,
            IUserService _userService,
            IMessageService _messageService
            )
        {
            this.repository = _repository;
            this.userService = _userService;
            this.messageService = _messageService;
        }

        public async Task<ChatViewModel> CreateAsync(string userAId, string userBId)
        {
            var userA = await this.userService.GetByIdAsync(userAId);
            var userB = await this.userService.GetByIdAsync(userBId);

            var chat = new Chat
            {
                Name = $"{userA.UserName} & {userB.UserName}",
                ImageUrl = ChatConstants.DefaultChatImage,
                IsGroupChat = false,
                LastMessage = "No messages yet",
                LastActive = DateTime.UtcNow.ToLocalTime(),
                CreatedOn = DateTime.UtcNow.ToLocalTime(),
                ModifiedOn = DateTime.UtcNow.ToLocalTime()
            };

            await this.repository.AddAsync(chat);

            var userChatA = new UserChat { UserId = userAId, ChatId = chat.Id };
            var userChatB = new UserChat { UserId = userBId, ChatId = chat.Id };

            await this.repository.AddAsync(userChatA);
            await this.repository.AddAsync(userChatB);

            await this.repository.SaveChangesAsync();

            ChatViewModel model = new ChatViewModel
            {
                Id = chat.Id,
                Name = chat.Name,
                ImageUrl = chat.ImageUrl,
                LastActive = DateHelper.TimeAgo(chat.LastActive),
                LastMessage = chat.LastMessage
            };

            return model;
        }

        public async Task<ChatViewModel> GetByIdAsync(Guid chatId)
        {
            Chat? chat = await this.repository.AllReadonly<Chat>()
                .FirstOrDefaultAsync(c => c.Id == chatId);

            ChatViewModel model = new ChatViewModel();

            if (chat == null)
            {
                return model;
            }

            model.Id = chat.Id;
            model.Name = chat.Name;
            model.ImageUrl = chat.ImageUrl;
            model.CreatedOn = DateHelper.GetDate(chat.CreatedOn);

            model.Participants = await this.userService.GetByChatAsync(chat.Id);

            model.Messages = await this.messageService.GetByChatId(chatId);

            return model;
        }

        public async Task<ICollection<ChatViewModel>> GetByUserAsync(string userId)
        {
            return await this.repository.AllReadonly<UserChat>()
                        .Where(uc => uc.UserId == userId)
                        .Include(uc => uc.Chat)
                        .Select(uc => uc.Chat)
                        .OrderBy(c => c.ModifiedOn ?? c.CreatedOn)
                        .Select(c => new ChatViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            ImageUrl = c.ImageUrl,
                            LastMessage = c.LastMessage == null
                                ? "No messages yet"
                                : c.LastMessage.Length < 30 ? c.LastMessage : c.LastMessage.Substring(0, 30) + "...",
                            LastActive = DateHelper.TimeAgo(c.LastActive)
                        })
                        .ToArrayAsync();
        }

        public async Task<bool> IsValidAsync(Guid chatId)
        {
            var chat = await this.repository.AllReadonly<Chat>()
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckIfChatExists(string userAId, string userBId)
        {
            var existingChat = await this.repository.AllReadonly<Chat>()
                .Include(c => c.UsersChats)
                .FirstOrDefaultAsync(c => c.UsersChats.Any(uc => uc.UserId == userAId) &&
                                           c.UsersChats.Any(uc => uc.UserId == userBId));

            if (existingChat == null)
            {
                return false;
            }

            return true;
        }

        public async Task<ChatViewModel> DeleteAsync(Guid chatId)
        {
            var chat = await this.repository.AllReadonly<Chat>()
                .FirstOrDefaultAsync(uc => uc.Id == chatId);

            if (chat == null)
            {
                throw new InvalidOperationException();
            }

            await this.repository.DeleteAsync<Chat>(chatId);

            await this.repository.SaveChangesAsync();

            return new ChatViewModel
            {
                Id = chatId,
                Name = chat.Name
            };
        }
    }
}
