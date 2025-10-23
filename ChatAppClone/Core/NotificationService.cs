namespace ChatAppClone.Core
{
    using System;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Models.ViewModels.Notifications;

    using ChatAppClone.Common.Constants;
    using ChatAppClone.Common.Helpers;
    using ChatAppClone.Common.Messages;

    public class NotificationService : INotificationService
    {
        private readonly IRepository repository;

        public NotificationService(IRepository _repository)
        {
            this.repository = _repository;
        }

        public async Task CreateAsync(string content, string url, string userId)
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

        public async Task DeleteAsync(Guid notificationId)
        {
            Notification? notification = await this.repository
                .AllReadonly<Notification>()
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
            {
                throw new InvalidOperationException(NotificationMessages.DoesNotExist);
            }

            await this.repository.DeleteAsync<Notification>(notification.Id);
            await this.repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationViewModel>> GetAsync(string userId, int currPage)
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
                                 Type = NotificationConstants.Type
                             })
                             .ToArrayAsync();

            return notifications;
        }

        public async Task<int> GetCountByUserId(string userId)
        {
            return await this.repository.AllReadonly<Notification>()
                .CountAsync(n => n.UserId == userId);
        }
    }
}
