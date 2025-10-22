namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Hubs;
    using ChatAppClone.Models.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

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
            var isExisting = await this.chatService.CheckIfChatExists(this.GetAuthId(), userToChatId);

            if (isExisting)
            {
                return this.RedirectToAction("Chats");
            }

            try
            {
                var chat = await this.chatService.CreateAsync(this.GetAuthId(), userToChatId);

                var followerUserName = User.Identity!.Name;

                await this.notificationService.CreateAsync(
                    $"{followerUserName} added you to chat.", string.Empty, userToChatId);

                await notificationHub.Clients.User(userToChatId).SendAsync("ReceiveNotification", $"{followerUserName} added you to chat.");

                await chatHub.Clients.User(userToChatId).SendAsync("StartChat", new
                {
                    id = chat.Id,
                    name = chat.Name,
                    imageUrl = chat.ImageUrl,
                    lastActive = chat.LastActive,
                    lastMessage = chat.LastMessage,
                    participantIds = chat.Participants.Select(p => p.Id).ToArray()
                });
            }
            catch (Exception)
            {
                this.RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            return this.RedirectToAction("Chats");
        }

        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            var chats = await this.chatService.GetByUserAsync(this.GetAuthId());

            GeneralChatViewModel model = new GeneralChatViewModel
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
                return this.RedirectToAction("Chats");
            }

            var chatModel = await this.chatService.GetByIdAsync(chatId.Value);

            ViewBag.CurrentUserId = this.GetAuthId();

            return this.PartialView("_ChatDetailsPartial", chatModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid? chatId)
        {
            if (!chatId.HasValue)
            {
                return this.RedirectToAction("Chats");
            }
           
            try
            {
                var participants = await this.userService.GetByChatAsync(chatId.Value);

                var deleted = await this.chatService.DeleteAsync(chatId.Value);

                await this.chatHub.Clients.Group(chatId.Value.ToString()).SendAsync("DeleteChat", chatId.Value);

                foreach (var participant in participants)
                {
                    await this.notificationHub.Clients.User(participant.Id).SendAsync("ReceiveNotification", $"Chat {deleted.Name} was deleted.");
                }
            }
            catch (Exception)
            {
                this.RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            return this.RedirectToAction("Chats");
        }
    }
}
