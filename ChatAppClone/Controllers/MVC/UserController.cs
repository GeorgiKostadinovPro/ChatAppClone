namespace ChatAppClone.Controllers.MVC
{
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Models.ViewModels.Users;

    using ChatAppClone.Utilities.Contracts;
    using ChatAppClone.Common.Pages;

    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(ICloudinaryService _cloudinaryService, IUserService _userService)
        {
            this.userService = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> Explore([FromQuery] ExploreUsersQueryModel model)
        {
            try
            {
                var currUserId = this.GetAuth().Item1;

                if (string.IsNullOrWhiteSpace(currUserId))
                {
                    return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
                }

                model.Users = await this.userService.GetAsync(currUserId, model);
                model.TotalUsersCount = await this.userService.GetCountAsync(model.SearchTerm);

                return this.View(model);
            }
            catch (Exception)
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { area = string.Empty, StatusCode = 404 });
            }
        }
    }
}
