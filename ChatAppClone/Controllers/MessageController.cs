namespace ChatAppClone.Controllers
{
    using ChatAppClone.Hubs;
    using Microsoft.AspNetCore.SignalR;

    public class MessageController : ApiController
    {
        private readonly IHubContext<ChatHub> hubContext;

        public MessageController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }
    }
}
