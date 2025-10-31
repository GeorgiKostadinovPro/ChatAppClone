namespace ChatAppClone.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using ChatAppClone.Hubs;
    using ChatAppClone.Core.Contracts;

    using ChatAppClone.Models.RequestModels;
    using ChatAppClone.Models.ViewModels.Messages;

    using ChatAppClone.Common.Messages;
    using ChatAppClone.Common.Constants;

    public class MessageApiController : ApiController
    {
        private readonly ILogger<MessageApiController> logger;

        private readonly IHubContext<ChatHub> chatHub;

        private readonly IChatService chatService;
        private readonly IUserService userService;
        private readonly IMessageService messageService;

        public MessageApiController(
            IHubContext<ChatHub> _chatHub, 
            ILogger<MessageApiController> _logger,
            IChatService _chatService, 
            IUserService _userService,
            IMessageService _messageService
            )
        {
            this.chatHub = _chatHub;
            this.logger = _logger;
            this.chatService = _chatService;
            this.userService = _userService;
            this.messageService = _messageService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MessageRequest request)
        {
            var currUserId = this.GetAuth().Item1;
            
            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.Unauthorized(UserMessages.InvalidUserId);
            }
            
            if (!await this.chatService.IsValidAsync(request.ChatId!.Value))
            {
                return this.BadRequest(ChatMessages.InvalidChatId);
            }
            
            MessageViewModel model = await this.messageService.CreateAsync(request.ChatId.Value, currUserId, request.Message!);
            
            await this.chatHub.Clients.Group(request.ChatId.Value.ToString()).SendAsync(ChatMessages.ReceiveMessage, new
            {
                creatorId = model.CreatorId,
                creatorProfilePictureUrl = (await this.userService.GetByIdAsync(currUserId)).ProfilePictureUrl,
                content = model.Content,
                createdOn = model.CreatedOn
            });

            this.logger.LogInformation(GeneralConstants.CreateMessageSuccessful);
            
            return this.Ok(model);
        }
    }
}
