namespace ChatAppClone.Controllers.MVC
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    
    [Authorize]
    public class BaseController : Controller 
    {
        protected (string, string) GetAuth()
        {
            string id = string.Empty;

            if (this.User.Identity!.IsAuthenticated
                && this.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                id = this.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!
                    .ToString();
            }

            return (id, this.User.Identity!.Name!);
        }
    }
}
