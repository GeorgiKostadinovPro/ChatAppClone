namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;

    public interface INotificationService
    {
        Task CreateNotificationAsync(string content, string url, string userId);

        Task DeleteNotificationAsync(string notificationId);

        Task<int> GetNotificationsCountByUserId(string userId);
    }
}
