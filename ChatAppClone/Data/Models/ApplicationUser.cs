namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<string>, IBaseEntityModel
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Messages = new HashSet<Message>();
            this.UsersChats = new HashSet<UserChat>();
            this.Followers = new HashSet<UserFollows>();
            this.Following = new HashSet<UserFollows>();
            this.Notifications = new HashSet<Notification>();
        }

        public string? ProfilePictureUrl { get; set; }

        public string? ProfilePicturePublicId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserChat> UsersChats { get; set; }

        public virtual ICollection<UserFollows> Followers { get; set; }

        public virtual ICollection<UserFollows> Following { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
