namespace ChatAppClone.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
