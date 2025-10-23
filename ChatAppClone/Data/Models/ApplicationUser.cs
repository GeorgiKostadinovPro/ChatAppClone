namespace ChatAppClone.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    using ChatAppClone.Data.Common.Models;
    
    public class ApplicationUser : IdentityUser<string>, IBaseEntityModel
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Messages = new HashSet<Message>();
            this.UsersChats = new HashSet<UserChat>();
            this.Notifications = new HashSet<Notification>();
        }

        public string? ProfilePictureUrl { get; set; }

        public string? ProfilePicturePublicId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserChat> UsersChats { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
