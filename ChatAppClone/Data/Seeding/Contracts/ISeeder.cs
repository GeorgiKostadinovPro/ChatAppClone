namespace ChatAppClone.Data.Seeding.Contracts
{
    public interface ISeeder
    {
        Task SeedAsync(ChatAppCloneDbContext dbContext, IServiceProvider serviceProvider);
    }
}
