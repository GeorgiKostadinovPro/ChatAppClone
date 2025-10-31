namespace ChatAppClone.Controllers.MVC
{
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Models.ViewModels.Users;

    using ChatAppClone.Common.Pages;
    using ChatAppClone.Common.Constants;

    public class UserController : BaseController
    {
        private readonly ILogger<UserController> logger;

        private readonly IUserService userService;

        public UserController(ILogger<UserController> _logger, IUserService _userService)
        {
            this.logger = _logger;

            this.userService = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> Explore([FromQuery] ExploreUsersQueryModel model)
        {
            var currUserId = this.GetAuth().Item1;

            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }

            model.Users = await this.userService.GetAsync(currUserId, model);
            model.TotalUsersCount = await this.userService.GetCountAsync(model.SearchTerm);

            this.logger.LogInformation(GeneralConstants.ExploreUsersSuccessful);

            return this.View(model);
        }
    }
}
