namespace ChatAppClone.Core
{
    using System;
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Models.ViewModels.Chats;
    using ChatAppClone.Models.ViewModels.Images;
    using ChatAppClone.Models.ViewModels.Messages;
    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IRepository repository;
        private readonly IUserService userService;

        public ChatService(IRepository _repository, IUserService _userService)
        {
            this.repository = _repository;
            this.userService = _userService;
        }

        public async Task<ChatViewModel> GetByIdAsync(Guid chatId)
        {
            Chat? chat = await this.repository.AllReadonly<Chat>()
                .Include(c => c.Messages)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            ChatViewModel model = new ChatViewModel();

            if (chat == null)
            {
                return model;
            }

            model.Id = chatId;
            model.Name = chat.Name;
            model.ImageUrl = chat.ImageUrl;
            model.CreatedOn = DateHelper.GetDate(chat.CreatedOn);
            model.LastActive = DateHelper.TimeAgo(chat.LastActive);

            model.Participants = await this.repository.AllReadonly<UserChat>()
                .Include(uc => uc.User)
                .Where(uc => uc.ChatId == chatId)
                .Select(uc => new ParticipantViewModel
                {
                    Id = uc.UserId,
                    ProfilePictureUrl = uc.User.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl
                }).ToArrayAsync();

            model.LastMessage = chat.LastMessage == null ? "No messages yet" : chat.LastMessage.Substring(0, 30) + "...";
            model.Messages = chat.Messages
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
                }).ToArray();

            model.Images = chat.Images.Select(i => new ImageViewModel
            {
                Id = i.Id,
                Url = i.Url
            }).ToArray();
            
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
                            LastMessage = c.LastMessage == null ? "No messages yet" : c.LastMessage.Substring(0, 30) + "...",
                            LastActive = DateHelper.TimeAgo(c.LastActive)
                        })
                        .ToArrayAsync();    
        }

        public async Task<Chat> CreateAsync(string userAId, string userBId)
        {
            var existingChat = await this.repository.AllReadonly<Chat>()
                .Include(c => c.UsersChats)
                .FirstOrDefaultAsync(c => c.UsersChats.Any(uc => uc.UserId == userAId) &&
                                           c.UsersChats.Any(uc => uc.UserId == userBId));

            if (existingChat != null)
            {
                return existingChat;
            }

            var userA = await this.userService.GetByIdAsync(userAId);
            var userB = await this.userService.GetByIdAsync(userBId);

            var chat = new Chat
            {
                Name = $"{userA.UserName} & {userB.UserName}",
                ImageUrl = userB.ProfilePictureUrl ?? UserConstants.DefaultProfilePictureUrl,
                IsGroupChat = false,
                LastActive = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            await this.repository.AddAsync(chat);

            var userChatA = new UserChat { UserId = userAId, ChatId = chat.Id };
            var userChatB = new UserChat { UserId = userBId, ChatId = chat.Id };

            await this.repository.AddAsync(userChatA);
            await this.repository.AddAsync(userChatB);

            await this.repository.SaveChangesAsync();

            return chat;
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
    }
}
