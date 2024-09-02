namespace ChatAppClone.Extensions
{
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
            
            return services;
        }
    }
}
