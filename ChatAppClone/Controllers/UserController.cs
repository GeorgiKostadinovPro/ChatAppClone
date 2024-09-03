namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CloudinaryDotNet.Actions;

    using ChatAppClone.Utilities.Contracts;
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Common.Messages;

    public class UserController : BaseController
    {
        private readonly ICloudinaryService cloudinaryService;

        private readonly IUserService userService;

        public UserController(ICloudinaryService _cloudinaryService, IUserService _userService)
        {
            this.cloudinaryService = _cloudinaryService;

            this.userService = _userService;
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
                ApplicationUser user = await this.userService.GetByIdAsync(this.GetAuthId());

                ImageUploadResult result = await this.cloudinaryService.UploadPictureAsync(file, CloudinaryConstants.ProfilePicturesFolder, user.ProfilePicturePublicId);

                await this.userService.SetProfilePictureAsync(this.GetAuthId(), result.SecureUrl.ToString(), result.PublicId);
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, error = ex.Message });
            }

            return this.Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            try
            {
                ApplicationUser user = await this.userService.GetByIdAsync(this.GetAuthId());

                await this.cloudinaryService.DeletePictureAsync(user.ProfilePicturePublicId!);

                await this.userService.DeleteProfilePictureAsync(this.GetAuthId());
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, error = ex.Message });
            }

            return this.Json(new { success = true });
        }
    }
}
