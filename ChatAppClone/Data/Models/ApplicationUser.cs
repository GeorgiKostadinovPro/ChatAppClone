namespace ChatAppClone.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();

            this.Messages = new HashSet<Message>();
            this.UsersChats = new HashSet<UserChat>();
        }

        public Guid? ProfilePictureId { get; set; }

        public virtual Image? ProfilePicture { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserChat> UsersChats { get; set; }
    }
}
