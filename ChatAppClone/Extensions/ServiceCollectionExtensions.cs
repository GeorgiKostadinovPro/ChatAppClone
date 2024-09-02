namespace ChatAppClone.Extensions
{
    using CloudinaryDotNet;

    using Microsoft.Extensions.Configuration;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
        {
            Account account = new Account(
                configuration["Cloudinary:CloudName"], 
                configuration["Cloudinary:APIKey"],
                configuration["Cloudinary:APISecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
            
            return services;
        }
    }
}
