namespace ChatAppClone.Areas.Identity.Pages.Account.Manage
{
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Notifications;
    using ChatAppClone.Models.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
    using System.ComponentModel.DataAnnotations;

    public class NotificationsModel : PageModel
    {
        private readonly INotificationService notificationService;

        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsModel(INotificationService _notificationService, UserManager<ApplicationUser> _userManager)
        {
            this.notificationService = _notificationService;
            this.userManager = _userManager;
        }

        [BindProperty]
        public QueryModel Model { get; set; }

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

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            int count = await this.notificationService.GetNotificationsCountByUserId(user.Id);

            Model = new QueryModel
            {
                TotalNotificationsCount = count,
            };
;
            return Page();
        }
    }
}
