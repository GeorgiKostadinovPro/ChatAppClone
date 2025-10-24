namespace ChatAppClone.Test.Helpers
{
    using Microsoft.EntityFrameworkCore;

    using ChatAppClone.Data;

    public static class InitializeInMemoryDatabase
    {
        public static async Task<ChatAppCloneDbContext> CreateInMemoryDatabase()
        {
            DbContextOptions<ChatAppCloneDbContext> _options = new DbContextOptionsBuilder<ChatAppCloneDbContext>()
                      .UseInMemoryDatabase(databaseName: "TestDb")
                      .Options;

            ChatAppCloneDbContext dbContext = new ChatAppCloneDbContext(_options);

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }
    }
}
