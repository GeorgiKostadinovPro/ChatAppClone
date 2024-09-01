namespace ChatAppClone.Extensions
{
    using System.Security.Claims;

    public static class UserClaimsPrincipalExtensions
    {
        public static string GetAuthId(this ClaimsPrincipal user)
        {
            string id = string.Empty;

            if (user!.Identity!.IsAuthenticated
                && user.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                id = user
                    .FindFirstValue(ClaimTypes.NameIdentifier)
                    .ToString();
            }

            return id;
        }
    }
}
