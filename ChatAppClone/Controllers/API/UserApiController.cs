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
        private readonly ILogger<UserApiController> logger;

        private readonly ICloudinaryService cloudinaryService;

        private readonly IUserService userService;

        public UserApiController(
            ILogger<UserApiController> _logger, 
            ICloudinaryService _cloudinaryService, 
            IUserService _userService
            )
        {
            this.logger = _logger;

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

            var currUserId = this.GetAuth().Item1;

            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }

            ApplicationUser user = await this.userService.GetByIdAsync(currUserId);

            ImageUploadResult result = await this.cloudinaryService
                .UploadPictureAsync(file, CloudinaryConstants.ProfilePicturesFolder, user.ProfilePicturePublicId);

            await this.userService.SetProfilePictureAsync(currUserId, result.SecureUrl.ToString(), result.PublicId);

            this.logger.LogInformation(GeneralConstants.UploadProfilePictureSuccessful);

            return this.Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var currUserId = this.GetAuth().Item1;
            
            if (string.IsNullOrWhiteSpace(currUserId))
            {
                return this.RedirectToAction(GeneralPages.Error, GeneralPages.Home, new { statusCode = 401 });
            }
            
            ApplicationUser user = await this.userService.GetByIdAsync(currUserId);
            
            await this.cloudinaryService.DeletePictureAsync(user.ProfilePicturePublicId!);
            
            await this.userService.DeleteProfilePictureAsync(currUserId);

            this.logger.LogInformation(GeneralConstants.DeleteProfilePictureSuccessful);

            return this.Ok(new { success = true });
        }
    }
}
