﻿namespace ChatAppClone.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Messages = new HashSet<Message>();
            this.UsersChats = new HashSet<UserChat>();
        }

        public string? ProfilePictureUrl { get; set; }

        public string? ProfilePicturePublicId { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<UserChat> UsersChats { get; set; }
    }
}
