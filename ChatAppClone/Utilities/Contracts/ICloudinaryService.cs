namespace ChatAppClone.Utilities.Contracts
{
    using CloudinaryDotNet.Actions;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadPictureAsync(IFormFile file, string folder, string? publicId = null);

        Task<DeletionResult> DeletePictureAsync(string publicId);

        bool IsFileValid(IFormFile formFile);
    }
}
