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

            if (string.IsNullOrEmpty(currUserId) || string.IsNullOrEmpty(userIdToFollow))
            {
                return BadRequest(new { message = "Invalid user data." });

            }

            var followed = await this.userFollowsService.FollowUserAsync(userIdToFollow, currUserId);

            if (!followed)
            {
                return BadRequest(new { message = "Already following." });
            }

            var followerUserName = User.Identity!.Name;

            await this.notificationService.CreateNotificationAsync(
                $"{followerUserName} has followed you.", "/User/Notifications/" + userIdToFollow, userIdToFollow);

            return Ok(new { message = "Followed successfully." });
        }

        [HttpPost("Unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] string userIdToUnfollow)
        {
            string currUserId = this.GetAuthId();

            if (string.IsNullOrEmpty(currUserId) || string.IsNullOrEmpty(userIdToUnfollow))
            {
                return BadRequest(new { message = "Invalid user data." });
            }

            var unfollowed = await this.userFollowsService.UnfollowUserAsync(userIdToUnfollow, currUserId);

            if (!unfollowed)
            {
                return BadRequest("You are not following this user.");
            }

            return Ok(new { message = "Followed successfully." });
        }
    }
}
