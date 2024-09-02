namespace ChatAppClone.Extensions
{
    using ChatAppClone.Utilities.Contracts;
    using ChatAppClone.Utilities;
    using CloudinaryDotNet;

    using Microsoft.Extensions.Configuration;

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

            return services;
        }
    }
}
