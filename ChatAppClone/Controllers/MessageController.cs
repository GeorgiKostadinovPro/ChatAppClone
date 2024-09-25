namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Hubs;
    using ChatAppClone.Models.RequestModels;
    using ChatAppClone.Models.ViewModels.Messages;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class MessageController : ApiController
    {
        private readonly IHubContext<ChatHub> hubContext;

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
            this.hubContext = hubContext;
            this.chatService = _chatService;
            this.userService = _userService;
            this.messageService = _messageService;
        }

        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage([FromBody] MessageRequest request)
        {
            if (!(await this.chatService.IsValidAsync(request.ChatId)))
            {
                return this.BadRequest("invalid chat id");
            }

            string currUserId = this.GetAuthId();

            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.BadRequest("invalid user Id");
            }

            MessageViewModel model = await this.messageService.CreateAsync(request.ChatId, currUserId, request.Message);

            await this.hubContext.Clients.Group(request.ChatId.ToString()).SendAsync("ReceiveMessage", new
            {
                creatorId = model.CreatorId,
                creatorProfilePictureUrl = (await userService.GetByIdAsync(currUserId)).ProfilePictureUrl,
                content = model.Content,
                createdOn = model.CreatedOn
            });

            return Ok(model);
        }
    }
}
