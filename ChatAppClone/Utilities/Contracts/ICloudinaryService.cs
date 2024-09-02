namespace ChatAppClone.Utilities.Contracts
{
    using CloudinaryDotNet.Actions;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file, string folder, string? publicId);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

        bool IsFileValid(IFormFile formFile);
    }
}
