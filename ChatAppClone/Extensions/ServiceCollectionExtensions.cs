namespace ChatAppClone.Extensions
{
    using CloudinaryDotNet;

    using Microsoft.Extensions.Configuration;

    using ChatAppClone.Core;
    using ChatAppClone.Core.Contracts;
    
    using ChatAppClone.Data.Repositories;

    using ChatAppClone.Utilities;
    using ChatAppClone.Utilities.Contracts;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            Account account = new Account(
                configuration["Cloudinary:CloudName"], 
                configuration["Cloudinary:APIKey"],
                configuration["Cloudinary:APISecret"]);

            Cloudinary cloud = new Cloudinary(account);

            services.AddSingleton(cloud);
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();

            return services;
        }
    }
}
