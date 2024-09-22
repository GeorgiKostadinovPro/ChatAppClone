namespace ChatAppClone.Models.ViewModels.Messages
{
    using ChatAppClone.Models.ViewModels.Images;

    public class MessageViewModel
    {
        public MessageViewModel()
        {
            this.MessageImages = new HashSet<ImageViewModel>();
        }

        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public bool IsSeen { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string CreatorId { get; set; } = null!;

        public IEnumerable<ImageViewModel> MessageImages { get; set; } 
    }
}
