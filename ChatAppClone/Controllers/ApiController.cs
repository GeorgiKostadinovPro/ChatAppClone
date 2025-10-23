namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase 
    {
        protected (string, string) GetAuth()
        {
            string id = string.Empty;

            if (this.User!.Identity!.IsAuthenticated
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
