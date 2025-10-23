namespace ChatAppClone.Controllers.API
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

            if (User!.Identity!.IsAuthenticated
                && User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                id = User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!
                    .ToString();
            }

            return (id, User.Identity!.Name!);
        }
    }
}
