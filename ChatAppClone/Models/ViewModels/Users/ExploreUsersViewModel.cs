namespace ChatAppClone.Models.ViewModels.Users
{
    public class ExploreUsersViewModel
    {
        public ExploreUsersViewModel()
        {
            this.Users = new HashSet<UserCardViewModel>();
        }

        public int TotalUsersCount { get; set; }

        public IEnumerable<UserCardViewModel> Users { get; set; }
    }
}
