namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UserFollowsController : ApiController
    {

        public UserFollowsController()
        {
            
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
