namespace ChatAppClone.Models.ViewModels.Chats
{
    using ChatAppClone.Models.ViewModels.Images;
    using ChatAppClone.Models.ViewModels.Messages;

    public class ChatViewModel
    {
        public ChatViewModel()
        {
            this.Images = new HashSet<ImageViewModel>();
            this.Messages = new HashSet<MessageViewModel>();
        }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public bool IsGroupChat { get; set; }

        public string? ImageUrl { get; set; }

        public string? LastMessage { get; set; }

        public DateTime LastActive { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
