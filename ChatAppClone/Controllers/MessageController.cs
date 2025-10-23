namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using ChatAppClone.Hubs;
    using ChatAppClone.Core.Contracts;
  
    using ChatAppClone.Models.RequestModels;
    using ChatAppClone.Models.ViewModels.Messages;

    using ChatAppClone.Common.Messages;

    public class MessageController : ApiController
    {
        private readonly IHubContext<ChatHub> chatHub;

        private readonly IChatService chatService;
        private readonly IUserService userService;
        private readonly IMessageService messageService;

        public MessageController(
            IHubContext<ChatHub> hubContext, 
            IChatService _chatService, 
            IUserService _userService,
            IMessageService _messageService
            )
        {
            this.chatHub = hubContext;
            this.chatService = _chatService;
            this.userService = _userService;
            this.messageService = _messageService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MessageRequest request)
        {
            try
            {
                var (currUserId, currUserName) = this.GetAuth();

                if (string.IsNullOrWhiteSpace(currUserId))
                {
                    return this.BadRequest(UserMessages.InvalidUserId);
                }

                if (!await this.chatService.IsValidAsync(request.ChatId.Value))
                {
                    return this.BadRequest(ChatMessages.InvalidChatId);
                }

                MessageViewModel model = await this.messageService.CreateAsync(request.ChatId.Value, currUserId, request.Message!);

                await this.chatHub.Clients.Group(request.ChatId.Value.ToString()).SendAsync(ChatMessages.ReceiveMessage, new
                {
                    creatorId = model.CreatorId,
                    creatorProfilePictureUrl = (await userService.GetByIdAsync(currUserId)).ProfilePictureUrl,
                    content = model.Content,
                    createdOn = model.CreatedOn
                });

                return this.Ok(model);
            }
            catch (Exception)
            {
                return this.BadRequest();
            }
        }
    }
}
