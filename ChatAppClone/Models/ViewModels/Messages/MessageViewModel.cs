namespace ChatAppClone.Models.ViewModels.Messages
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public bool IsSeen { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string CreatorId { get; set; } = null!;
    }
}
