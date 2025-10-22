namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BaseController : Controller 
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
