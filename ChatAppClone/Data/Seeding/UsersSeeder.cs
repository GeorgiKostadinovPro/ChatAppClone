namespace ChatAppClone.Data.Seeding
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Seeding.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

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
                UserName = "Go4ko",
                Email = "georgi.kostadinov14@abv.bg",
                EmailConfirmed = true,
                ProfilePictureUrl = "https://res.cloudinary.com/de1i8aava/image/upload/v1726509898/ChatAppClone/assets/user-profile-pictures/profile-picture.jpg",
                ProfilePicturePublicId = "ChatAppClone/assets/user-profile-pictures/profile-picture"
            };

            ApplicationUser user2 = new ApplicationUser
            {
                UserName = "Lyub4o",
                Email = "user@mail.com",
                EmailConfirmed = true,
                ProfilePictureUrl = "https://res.cloudinary.com/de1i8aava/image/upload/v1725385674/ChatAppClone/assets/user-profile-pictures/profil1.jpg",
                ProfilePicturePublicId = "ChatAppClone/assets/user-profile-pictures/profil1"
            };

            string password = "Gk123.";

            await userManager.CreateAsync(user1, password);
            await userManager.CreateAsync(user2, password);
        }
    }
}
