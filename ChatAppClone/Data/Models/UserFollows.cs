namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class UserFollows : BaseEntityModel<Guid>
    {
        public UserFollows()
        {
            this.Id = Guid.NewGuid();
        }

        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public string FollowerId { get; set; } = null!;

        public virtual ApplicationUser Follower { get; set; } = null!;

        public bool IsConfirmed { get; set; }
    }
}
