namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Hubs;
    using ChatAppClone.Models.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using ChatAppClone.Common.Pages;
    using ChatAppClone.Common.Messages;

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
            try
            {
                var (currUserId, currentUserName) = this.GetAuth();

                if (string.IsNullOrWhiteSpace(currUserId) || string.IsNullOrWhiteSpace(currentUserName))
                {
                    return this.BadRequest(UserMessages.InvalidUserId);
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
            catch (Exception)
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 404 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            try
            {
                var (currUserId, currUserName) = this.GetAuth();

                if (string.IsNullOrWhiteSpace(currUserId) || string.IsNullOrWhiteSpace(currUserName))
                {
                    return BadRequest(UserMessages.InvalidUserId);
                }

                var chats = await this.chatService.GetByUserAsync(currUserId);

                var model = new GeneralChatViewModel
                {
                    Chats = chats
                };

                return this.View(model);
            }
            catch (Exception ex)
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Load(Guid? chatId)
        {
            if (!chatId.HasValue)
            {
                return this.RedirectToAction(ChatPages.Chats);
            }

            try
            {
                var (currUserId, currUserName) = this.GetAuth();

                if (string.IsNullOrWhiteSpace(currUserId) || string.IsNullOrWhiteSpace(currUserName))
                {
                    return BadRequest(UserMessages.InvalidUserId);
                }

                ViewBag.CurrentUserId = currUserId;

                var chatModel = await this.chatService.GetByIdAsync(chatId.Value);

                return this.PartialView("_ChatDetailsPartial", chatModel);
            }
            catch (Exception)
            {
                return this.RedirectToAction(ChatPages.Chats);
            }
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

                await this.chatHub.Clients.Group(chatId.Value.ToString())
                    .SendAsync(ChatMessages.DeleteChat, chatId.Value);

                foreach (var participant in participants)
                {
                    await this.notificationHub.Clients.User(participant.Id)
                        .SendAsync(NotificationMessages.ReceiveNotification, string.Format(ChatMessages.ChatWasDeleted, deleted.Name));
                }

                return this.RedirectToAction(ChatPages.Chats);
            }
            catch (Exception)
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 404 });
            }
        }
    }
}
