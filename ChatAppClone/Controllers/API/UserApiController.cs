namespace ChatAppClone.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;

    using CloudinaryDotNet.Actions;

    using ChatAppClone.Data.Models;
    using ChatAppClone.Core.Contracts;

    using ChatAppClone.Utilities.Contracts;

    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Messages;
    using ChatAppClone.Common.Pages;

    public class UserApiController : ApiController
    {
        private readonly ICloudinaryService cloudinaryService;

        private readonly IUserService userService;

        public UserApiController(ICloudinaryService _cloudinaryService, IUserService _userService)
        {
            this.cloudinaryService = _cloudinaryService;

            this.userService = _userService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile? file)
        {
            if (file == null)
            {
                return this.BadRequest(new { success = false, error = UserMessages.ChooseFile });
            }

            if (!this.cloudinaryService.IsFileValid(file))
            {
                return this.BadRequest(new { success = false, error = UserMessages.ValidTypes });
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

                return this.Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, error = ex.Message });
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

                return this.Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
