﻿namespace ChatAppClone.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using ChatAppClone.Hubs;
    using ChatAppClone.Core.Contracts;

    using ChatAppClone.Common.Messages;

    public class ChatApiController : ApiController
    {
        private readonly IHubContext<ChatHub> chatHub;
        private readonly IHubContext<NotificationHub> notificationHub;

        private readonly IUserService userService;
        private readonly IChatService chatService;
        private readonly INotificationService notificationService;

        public ChatApiController(
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

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Guid? chatId)
        {
            if (!chatId.HasValue)
            {
                return this.BadRequest();
            }

            try
            {
                var participants = await this.userService.GetByChatAsync(chatId.Value);

                var deleted = await this.chatService.DeleteAsync(chatId.Value);

                await this.chatHub.Clients.Group(chatId.Value.ToString())
                    .SendAsync(ChatMessages.DeleteChat, chatId.Value);

                foreach (var participant in participants)
                {
                    await this.notificationService
                        .CreateAsync(NotificationMessages.UserDeletedChat, string.Empty, participant.Id);

                    await this.notificationHub.Clients.User(participant.Id)
                        .SendAsync(NotificationMessages.ReceiveNotification, string.Format(ChatMessages.ChatWasDeleted, deleted.Name));
                }

                return this.Ok();
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }
    }
}
