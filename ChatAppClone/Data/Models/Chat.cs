namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class Chat : BaseEntityModel<Guid>
    {
        public Chat()
        {
            this.Id = Guid.NewGuid();

            this.Images = new HashSet<Image>();
            this.Messages = new HashSet<Message>();
            this.UsersChats = new HashSet<UserChat>();
        }

        public string? Name { get; set; }

        public bool IsGroupChat { get; set; }

        public string? ImageUrl { get; set; }

        public string? LastMessage { get; set; }

        public DateTime LastActive { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserChat> UsersChats { get; set; }
    }
}
