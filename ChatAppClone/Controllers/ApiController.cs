namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase 
    {
        protected string GetAuthId()
        {
            string id = string.Empty;

            if (this.User!.Identity!.IsAuthenticated
                && this.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                id = this.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!
                    .ToString();
            }

            return id;
        }
    }
}
