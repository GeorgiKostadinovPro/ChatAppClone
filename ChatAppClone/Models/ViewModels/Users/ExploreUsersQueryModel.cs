namespace ChatAppClone.Models.ViewModels.Users
{
    using ChatAppClone.Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class ExploreUsersQueryModel
    {
        public ExploreUsersQueryModel()
        {
            this.UsersPerPage = UserConstants.UsersPerPage;
            this.CurrentPage = UserConstants.DefaultPage;

            this.Users = new HashSet<UserCardViewModel>();
        }

        [Display(Name = "Type anything...")]
        public string? SearchTerm { get; set; }

        public int UsersPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalUsersCount { get; set; }

        public IEnumerable<UserCardViewModel> Users { get; set; }
    }
}
