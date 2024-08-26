namespace ChatAppClone.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
