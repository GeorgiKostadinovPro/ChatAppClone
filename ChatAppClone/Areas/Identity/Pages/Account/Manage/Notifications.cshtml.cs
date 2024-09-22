namespace ChatAppClone.Areas.Identity.Pages.Account.Manage
{
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Notifications;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class NotificationsModel : PageModel
    {
        private readonly INotificationService notificationService;

        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsModel(INotificationService _notificationService, UserManager<ApplicationUser> _userManager)
        {
            this.notificationService = _notificationService;
            this.userManager = _userManager;

            this.Query = new QueryModel();
        }

        [BindProperty]
        public QueryModel Query { get; set; } = null!;

        public class QueryModel
        {
            public QueryModel()
            {
                this.NotificationsPerPage = NotificationConstants.NotificationsPerPage;
                this.CurrentPage = NotificationConstants.DefaultPage;

                this.Notifications = new HashSet<NotificationViewModel>();
            }

            public int NotificationsPerPage { get; set; }

            public int CurrentPage { get; set; }

            public int TotalNotificationsCount { get; set; }

            public IEnumerable<NotificationViewModel> Notifications { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? currentPage)
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            Query.CurrentPage = currentPage ?? 1;

            int count = await this.notificationService.GetCountByUserId(user.Id);
            var notifications = await this.notificationService.GetAsync(user.Id, Query.CurrentPage);

            Query.TotalNotificationsCount = count;
            Query.Notifications = notifications;

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteNotificationAsync(Guid notificationId)
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            await this.notificationService.DeleteAsync(notificationId);

            return RedirectToPage();
        }
    }
}
