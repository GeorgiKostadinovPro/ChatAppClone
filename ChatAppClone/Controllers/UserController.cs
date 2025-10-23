namespace ChatAppClone.Controllers
{
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Messages;
    using ChatAppClone.Common.Pages;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Users;
    using ChatAppClone.Utilities.Contracts;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseController
    {
        private readonly ICloudinaryService cloudinaryService;

        private readonly IUserService userService;

        public UserController(ICloudinaryService _cloudinaryService, IUserService _userService)
        {
            this.cloudinaryService = _cloudinaryService;

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

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null)
            {
                return this.Json(new { success = false, error = UserMessages.ChooseFile });
            }

            if (!this.cloudinaryService.IsFileValid(file))
            {
                return this.Json(new { success = false, error = UserMessages.ValidTypes });
            }

            try
            {
                var currUserId = this.GetAuth().Item1;

                if (string.IsNullOrWhiteSpace(currUserId))
                {
                    return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
                }

                ApplicationUser user = await this.userService.GetByIdAsync(currUserId);

                ImageUploadResult result = await this.cloudinaryService
                    .UploadPictureAsync(file, CloudinaryConstants.ProfilePicturesFolder, user.ProfilePicturePublicId);

                await this.userService.SetProfilePictureAsync(currUserId, result.SecureUrl.ToString(), result.PublicId);

                return this.Json(new { success = true });
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            try
            {
                var currUserId = this.GetAuth().Item1;

                if (string.IsNullOrWhiteSpace(currUserId))
                {
                    return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
                }

                ApplicationUser user = await this.userService.GetByIdAsync(currUserId);

                await this.cloudinaryService.DeletePictureAsync(user.ProfilePicturePublicId!);

                await this.userService.DeleteProfilePictureAsync(currUserId);

                return this.Json(new { success = true });
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, error = ex.Message });
            }
        }
    }
}
