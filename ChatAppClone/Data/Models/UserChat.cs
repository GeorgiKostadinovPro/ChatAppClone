namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class UserChat : BaseEntityModel<Guid>
    {
        public UserChat()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public Guid ChatId { get; set; }

        public virtual Chat Chat { get; set; } = null!;
    }
}
