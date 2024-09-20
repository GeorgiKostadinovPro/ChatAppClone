namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class Notification : BaseEntityModel<Guid>
    {
        public Notification()
        {
            this.Id = Guid.NewGuid();
        }

        public string? UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Url { get; set; }

        public bool IsRead { get; set; }
    }
}
