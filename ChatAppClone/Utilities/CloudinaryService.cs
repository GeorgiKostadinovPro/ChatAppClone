namespace ChatAppClone.Utilities
{   
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using ChatAppClone.Common.Messages;
    using ChatAppClone.Utilities.Contracts;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary _cloudinary)
        {
            this.cloudinary = _cloudinary;
        }

        public async Task<ImageUploadResult> UploadPictureAsync(IFormFile file, string folder, string? publicId = null)
        {
            ImageUploadResult result = new ImageUploadResult();

            if (file.Length > 0)
            {
                string fullPublicId;

                if (string.IsNullOrWhiteSpace(publicId))
                {
                    fullPublicId = folder + "/" + Path.GetFileNameWithoutExtension(file.FileName);
                }
                else
                {
                    if (!publicId.StartsWith(folder + "/"))
                    {
                        fullPublicId = folder + "/" + publicId;
                    }
                    else
                    {
                        fullPublicId = publicId;
                    }
                }
                
                using Stream stream = file.OpenReadStream();

                ImageUploadParams imageUploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = fullPublicId,
                    Overwrite = !string.IsNullOrWhiteSpace(publicId)
                }; 

                result = await this.cloudinary.UploadAsync(imageUploadParams);
            }

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException(CloudinaryMessages.NOTUploadedSuccessfully);
            }

            return result;
        }

        public async Task<DeletionResult> DeletePictureAsync(string publicId)
        {
            DeletionParams delParams = new DeletionParams(publicId);

            DeletionResult result = await this.cloudinary.DestroyAsync(delParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException(CloudinaryMessages.NOTDeletedSuccessfully);
            }

            return result;
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
