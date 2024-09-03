namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Utilities.Contracts;
    using ChatAppClone.Common.Constants;
    using CloudinaryDotNet.Actions;
    using ChatAppClone.Core.Contracts;

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
                return this.Json(new { success = false, error = "Please, choose a file." });
            }

            if (!this.cloudinaryService.IsFileValid(file))
            {
                return this.Json(new { success = false, error = "The valid types are .jpg, .jpeg, .png" });
            }

            try
            {
                ImageUploadResult result = await this.cloudinaryService.UploadPictureAsync(file, CloudinaryConstants.ProfilePicturesFolder);

                await this.userService.SetUserProfilePictureAsync(this.GetAuthId(), result.SecureUrl.ToString(), result.PublicId);
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, error = ex.Message });
            }

            return this.Json(new { success = true });
        }
    }
}
