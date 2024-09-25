namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class Message : BaseEntityModel<Guid>
    { 
        public Message()
        {
            this.Id = Guid.NewGuid();
        }

        public string Content { get; set; } = null!;

        public bool IsSeen { get; set; }

        public string CreatorId { get; set; } = null!;

        public virtual ApplicationUser Creator { get; set; } = null!;

        public Guid ChatId { get; set; }

        public virtual Chat Chat { get; set; } = null!;
    }
}
