namespace ChatAppClone.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
 
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Seeding.Contracts;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ChatAppCloneDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Users.AnyAsync())
            {
                return;
            }

            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            ApplicationUser user1 = new ApplicationUser
            {
                UserName = "Georgi",
                Email = "georgi@mail.com",
                EmailConfirmed = true,
                ProfilePictureUrl = "https://res.cloudinary.com/de1i8aava/image/upload/v1726509898/ChatAppClone/assets/user-profile-pictures/profile-picture.jpg",
                ProfilePicturePublicId = "ChatAppClone/assets/user-profile-pictures/profile-picture",
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };

            ApplicationUser user2 = new ApplicationUser
            {
                UserName = "Lyuboslav",
                Email = "lyubo@mail.com",
                EmailConfirmed = true,
                ProfilePictureUrl = "https://res.cloudinary.com/de1i8aava/image/upload/v1725385674/ChatAppClone/assets/user-profile-pictures/profil1.jpg",
                ProfilePicturePublicId = "ChatAppClone/assets/user-profile-pictures/profil1",
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };

            ApplicationUser user3 = new ApplicationUser
            {
                UserName = "Kris4o",
                Email = "kris@mail.com",
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };

            ApplicationUser user4 = new ApplicationUser
            {
                UserName = "Peter",
                Email = "peter@mail.com",
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };

            ApplicationUser user5 = new ApplicationUser
            {
                UserName = "Wang",
                Email = "wang@mail.com",
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.ToLocalTime()
            };

            string password = "User123.";

            await userManager.CreateAsync(user1, password);
            await userManager.CreateAsync(user2, password);
            await userManager.CreateAsync(user3, password);
            await userManager.CreateAsync(user4, password);
            await userManager.CreateAsync(user5, password);
        }
    }
}
