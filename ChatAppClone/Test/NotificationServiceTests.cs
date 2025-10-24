namespace ChatAppClone.Test
{
    using Microsoft.EntityFrameworkCore;

    using Moq;
    using NUnit.Framework;

    using ChatAppClone.Core;
    using ChatAppClone.Data;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using ChatAppClone.Test.Helpers;
    
    [TestFixture]
    public class NotificationServiceTests
    {
        private ChatAppCloneDbContext dbContext;

        private Mock<IRepository> mockRepository;

        [SetUp]
        public async Task Setup()
        {
            this.mockRepository = new Mock<IRepository>();

            this.dbContext = await InitializeInMemoryDatabase.CreateInMemoryDatabase();
        }

        [Test]
        public async Task Test_CreateNotificationShouldWorkProperly()
        {
            string param = "Test notification.";

            var service = new NotificationService(this.mockRepository.Object);

            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Notification>()))
                .Callback(async (Notification notification) =>
                {
                    await this.dbContext.Notifications.AddAsync(notification);
                    await this.dbContext.SaveChangesAsync();
                });

            await service.CreateAsync(param, param, param);

            var count = await this.dbContext.Notifications.CountAsync();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_DeleteNotificationShouldWorkProperly()
        {
            string param = "Test notification.";

            var service = new NotificationService(this.mockRepository.Object);

            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Notification>()))
                .Callback(async (Notification notification) =>
                {
                    await this.dbContext.Notifications.AddAsync(notification);
                    await this.dbContext.SaveChangesAsync();
                });

            await service.CreateAsync(param, param, param);

            var notification = await this.dbContext.Notifications.FirstOrDefaultAsync();

            Assert.That(notification, Is.Not.Null);

            this.mockRepository
                .Setup(x => x.AllReadonly<Notification>())
                .Returns(dbContext.Notifications.AsQueryable());

            this.mockRepository
                .Setup(x => x.DeleteAsync<Notification>(It.IsAny<object>()))
                .Callback((object id) => 
                {
                    var notification = this.dbContext.Notifications.FirstOrDefault(n => n.Id == (Guid)id);
                    if (notification != null)
                    {
                        this.dbContext.Notifications.Remove(notification);
                        this.dbContext.SaveChanges();
                    }
                })
                .Returns(Task.CompletedTask);

            await service.DeleteAsync(notification!.Id);

            notification = await this.dbContext.Notifications.FirstOrDefaultAsync();

            Assert.That(notification, Is.Null);
        }

        [Test]
        public async Task Test_DeleteNotificationShouldThrowWhenNull()
        {
            string param = "Test notification.";

            var service = new NotificationService(this.mockRepository.Object);

            await this.dbContext.Notifications.AddRangeAsync(new List<Notification> {         
                new Notification
                {
                    UserId = param,
                    Content = param,
                    Url = param,
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                },
                new Notification
                {
                    UserId = param,
                    Content = param,
                    Url = param,
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                }
            });

            await this.dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await service.DeleteAsync(Guid.NewGuid());
            });
        }
    }
}
