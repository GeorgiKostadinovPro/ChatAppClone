namespace ChatAppClone.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Utilities.Contracts;
    using ChatAppClone.Common.Constants;

    public class UserController : BaseController
    {
        private readonly ICloudinaryService cloudinaryService;

        public UserController(ICloudinaryService _cloudinaryService)
        {
            this.cloudinaryService = _cloudinaryService;
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

            // Your file processing logic here
            await this.cloudinaryService.UploadPictureAsync(file, CloudinaryConstants.ProfilePicturesFolder);

            // Return success message as JSON
            return this.Json(new { success = true });
        }
    }
}
