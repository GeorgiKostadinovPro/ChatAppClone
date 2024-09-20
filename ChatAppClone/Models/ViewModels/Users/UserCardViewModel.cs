namespace ChatAppClone.Models.ViewModels.Users
{
    public class UserCardViewModel
    {
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public string CreatedOn { get; set; } = null!;

        public int FollowersCount { get; set; }
    }
}
