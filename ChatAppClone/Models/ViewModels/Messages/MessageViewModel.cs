namespace ChatAppClone.Models.ViewModels.Messages
{
    using ChatAppClone.Models.ViewModels.Images;

    public class MessageViewModel
    {
        public MessageViewModel()
        {
            this.MessageImages = new HashSet<ImageViewModel>();
        }

        public string Content { get; set; } = null!;

        public bool IsSeen { get; set; }

        public string CreatorId { get; set; } = null!;

        public IEnumerable<ImageViewModel> MessageImages { get; set; } 
    }
}
