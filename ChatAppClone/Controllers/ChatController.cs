namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : BaseController
    {
        public ChatController() {}

        [HttpGet]
        public IActionResult Chat()
        {
            return this.View();
        }
    }
}
