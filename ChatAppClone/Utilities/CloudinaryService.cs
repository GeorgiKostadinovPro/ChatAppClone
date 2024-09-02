namespace ChatAppClone.Utilities
{
    using ChatAppClone.Utilities.Contracts;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary _cloudinary)
        {
            this.cloudinary = _cloudinary;
        }
        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file, string folder, string? publicId)
        {
            ImageUploadResult result = new ImageUploadResult();

            if (file.Length > 0)
            {
                using Stream stream = file.OpenReadStream();

                ImageUploadParams imageUploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder
                };

                if (publicId != null)
                {
                    imageUploadParams.PublicId = publicId;
                    imageUploadParams.Overwrite = true;
                }

                result = await this.cloudinary.UploadAsync(imageUploadParams);
            }

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Picture was NOT uploaded successfully!");
            }

            return result;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            throw new NotImplementedException();
        }

        public bool IsFileValid(IFormFile formFile)
        {
            if (formFile == null)
            {
                return false;
            }

            string extension = Path.GetExtension(formFile.FileName);

            string[] validExtensions = { ".jpg", ".jpeg", ".png" };

            if (!validExtensions.Contains(extension))
            {
                return false;
            }

            return true;
        }
    }
}
