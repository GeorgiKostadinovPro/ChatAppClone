namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Models.ViewModels.Chats;
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : BaseController
    {
        private readonly IUserService userService;
        private readonly IChatService chatService;

        public ChatController(IUserService _userService, IChatService _chatService) 
        {
            this.userService = _userService;
            this.chatService = _chatService;
        }

        [HttpGet]
        public async Task<IActionResult> Chat(Guid? chatId)
        {
            var chats = await this.chatService.GetChatsByUserAsync(this.GetAuthId());
            
            GeneralChatViewModel model = new GeneralChatViewModel
            {
                Chats = chats
            };

            if (!chatId.HasValue)
            {
                return this.View(model);
            }

            var chat = await this.chatService.GetChatByIdAsync(chatId.Value);
            
            model.Chat = chat;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StartChat(string userToChatId)
        {
            var chat = await this.chatService.CreateChatAsync(this.GetAuthId(), userToChatId);

            if (chat == null)
            {
                return BadRequest("Chat could not be created.");
            }

            return RedirectToAction("Chat", new { chatId = chat.Id });
        }
    }
}
