namespace ChatAppClone.Controllers.MVC
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    
    using ChatAppClone.Hubs;
    using ChatAppClone.Core.Contracts;
    
    using ChatAppClone.Models.ViewModels.Chats;

    using ChatAppClone.Common.Messages;
    using ChatAppClone.Common.Pages;

    public class ChatController : BaseController
    {
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IHubContext<NotificationHub> notificationHub;

        private readonly IUserService userService;
        private readonly IChatService chatService;
        private readonly INotificationService notificationService;

        public ChatController(
            IHubContext<ChatHub> _chatHub,
            IHubContext<NotificationHub> _hubContext,
            IUserService _userService,
            IChatService _chatService,
            INotificationService _notificationService
            )
        {
            this.chatHub = _chatHub;
            this.notificationHub = _hubContext;
            this.userService = _userService;
            this.chatService = _chatService;
            this.notificationService = _notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Start(string userToChatId)
        {
            var (currUserId, currentUserName) = this.GetAuth();
            
            if (string.IsNullOrWhiteSpace(currUserId) || string.IsNullOrWhiteSpace(currentUserName))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }
            
            if (await this.chatService.CheckIfChatExists(currUserId, userToChatId))
            {
                return this.RedirectToAction(ChatPages.Chats);
            }
            
            var chat = await this.chatService.CreateAsync(currUserId, userToChatId);
            
            await this.notificationService
                .CreateAsync(string.Format(NotificationMessages.UserAddedYouToChat, currentUserName), string.Empty, userToChatId);
            
            await this.notificationHub.Clients.User(userToChatId)
                .SendAsync(NotificationMessages.ReceiveNotification, string.Format(NotificationMessages.UserAddedYouToChat, currentUserName));
            
            await this.chatHub.Clients.User(userToChatId).SendAsync(ChatMessages.StartChat, new
            {
                id = chat.Id,
                name = chat.Name,
                imageUrl = chat.ImageUrl,
                lastActive = chat.LastActive,
                lastMessage = chat.LastMessage,
                participantIds = chat.Participants.Select(p => p.Id).ToArray()
            });
            
            return this.RedirectToAction(ChatPages.Chats);
        }

        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            var currUserId = this.GetAuth().Item1;
            
            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }
            
            var chats = await this.chatService.GetByUserAsync(currUserId);
            
            var model = new GeneralChatViewModel
            {
                Chats = chats
            };
            
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Load(Guid? chatId)
        {
            if (!chatId.HasValue)
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 404 });
            }

            var currUserId = this.GetAuth().Item1;
            
            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }
            
            ViewBag.CurrentUserId = currUserId;
            
            var chatModel = await this.chatService.GetByIdAsync(chatId.Value);
            
            return this.PartialView("_ChatDetailsPartial", chatModel);
        }
    }
}
