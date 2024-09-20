namespace ChatAppClone.Controllers
{
    using ChatAppClone.Core.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class UserFollowsController : ApiController
    {
        private readonly IUserFollowsService userFollowsService;
        private readonly INotificationService notificationService;

        public UserFollowsController(IUserFollowsService _userFollowsService, INotificationService _notificationService)
        {
            this.userFollowsService = _userFollowsService;
            this.notificationService = _notificationService;
        }

        [HttpPost("Follow")]
        public async Task<IActionResult> Follow([FromBody] string userIdToFollow)
        {
            string currUserId = this.GetAuthId();

            throw new NotImplementedException();
        }

        public async Task<IActionResult> Unfollow([FromBody] string userIdToUnfollow)
        {
            string currUserId = this.GetAuthId();

            throw new NotImplementedException();
        }
    }
}
