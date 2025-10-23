namespace ChatAppClone.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using ChatAppClone.Data.Seeding.Contracts;

    public class ChatAppCloneDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ChatAppCloneDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(typeof(ChatAppCloneDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                            new UsersSeeder()
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger?.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
