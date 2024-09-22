namespace ChatAppClone.Core
{
    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Models.ViewModels.Notifications;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class NotificationService : INotificationService
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationService(IRepository _repository, UserManager<ApplicationUser> _userManager)
        {
            this.repository = _repository;
            this.userManager = _userManager;
        }

        public async Task CreateNotificationAsync(string content, string url, string userId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Content = content,
                Url = url,
                IsRead = false,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            await this.repository.AddAsync(notification);
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            Notification? notification = await this.repository
                .AllReadonly<Notification>()
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification != null)
            {
                await this.repository.DeleteAsync<Notification>(notification.Id);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsAsync(string userId, int currPage)
        {
            IEnumerable<NotificationViewModel> notifications
                = await this.repository.AllReadonly<Notification>()
                             .Where(n => n.UserId == userId)
                             .Skip((currPage - 1) * NotificationConstants.NotificationsPerPage)
                             .Take(NotificationConstants.NotificationsPerPage)
                             .Select(n => new NotificationViewModel
                             {
                                 Id = n.Id,
                                 Content = n.Content,
                                 CreatedOn = DateHelper.GetDate(n.CreatedOn),
                                 Type = "Info"
                             })
                             .ToArrayAsync();

            return notifications;
        }

        public async Task<int> GetNotificationsCountByUserId(string userId)
        {
            return await this.repository.AllReadonly<Notification>()
                .CountAsync(n => n.UserId == userId);
        }
    }
}
