namespace ChatAppClone.Core
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class ChatService : IChatService
    {
        private readonly IRepository repository;

        public ChatService(IRepository _repository)
        {
            this.repository = _repository;
        }

        public async Task<Chat?> GetChatByIdAsync(Guid chatId)
        {
            Chat? chat = await this.repository.AllReadonly<Chat>()
                .Include(c => c.Messages)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            return chat;
        }

        public async Task<ICollection<Chat>> GetChatsByUserAsync(string userId)
        {
            return await this.repository.AllReadonly<UserChat>()
                        .Where(uc => uc.UserId == userId)
                        .Include(uc => uc.Chat)
                            .ThenInclude(c => c.Messages)
                        .Include(uc => uc.Chat)
                            .ThenInclude(c => c.Images)
                        .Select(uc => uc.Chat)
                        .OrderBy(c => c.ModifiedOn ?? c.CreatedOn)
                        .ToArrayAsync();    
        }

        public async Task<Chat> CreateChatAsync(string userAId, string userBId)
        {
            var existingChat = await this.repository.AllReadonly<Chat>()
                .Include(c => c.UsersChats)
                .FirstOrDefaultAsync(c => c.UsersChats.Any(uc => uc.UserId == userAId) &&
                                           c.UsersChats.Any(uc => uc.UserId == userBId));

            if (existingChat != null)
            {
                return existingChat;
            }

            var chat = new Chat
            {
                Name = $"{userAId} & {userBId}",
                IsGroupChat = false,
                LastActive = DateTime.UtcNow
            };

            await this.repository.AddAsync(chat);

            var userChatA = new UserChat { UserId = userAId, ChatId = chat.Id };
            var userChatB = new UserChat { UserId = userBId, ChatId = chat.Id };

            await this.repository.AddAsync(userChatA);
            await this.repository.AddAsync(userChatB);

            await this.repository.SaveChangesAsync();

            return chat;
        }
    }
}
