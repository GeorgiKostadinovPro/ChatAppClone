﻿namespace ChatAppClone.Models.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }

        public string Type { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;
    }
}
