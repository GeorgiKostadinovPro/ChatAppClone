﻿namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Hubs;
    using ChatAppClone.Models.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class ChatController : BaseController
    {
        private readonly IHubContext<NotificationHub> hubContext;

        private readonly IUserService userService;
        private readonly IChatService chatService;
        private readonly INotificationService notificationService;

        public ChatController(
            IHubContext<NotificationHub> hubContext, 
            IUserService _userService, 
            IChatService _chatService, 
            INotificationService _notificationService
        ) 
        {
            this.hubContext = hubContext;
            this.userService = _userService;
            this.chatService = _chatService;
            this.notificationService = _notificationService;
        }
        
        [HttpGet]
        public async Task<IActionResult> StartChat(string userToChatId)
        {
            var chat = await this.chatService.CreateChatAsync(this.GetAuthId(), userToChatId);

            if (chat == null)
            {
                return BadRequest("Chat could not be created.");
            }

            var followerUserName = User.Identity!.Name;

            await this.notificationService.CreateNotificationAsync(
                $"{followerUserName} added you to chat.", string.Empty, userToChatId);

            await hubContext.Clients.User(userToChatId).SendAsync("ReceiveNotification", $"{followerUserName} added you to chat.");

            return RedirectToAction("Chats");
        }

        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            var chats = await this.chatService.GetChatsByUserAsync(this.GetAuthId());
            
            GeneralChatViewModel model = new GeneralChatViewModel
            {
                Chats = chats
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadChat(Guid? chatId)
        {
            if (!chatId.HasValue)
            {
                return this.RedirectToAction("Chats");
            }

            var chatModel = await this.chatService.GetChatByIdAsync(chatId.Value);

            return this.PartialView("_ChatDetailsPartial", chatModel);
        }
    }
}
