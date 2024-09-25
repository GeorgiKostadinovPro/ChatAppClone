namespace ChatAppClone.Models.ViewModels.Chats
{
    using ChatAppClone.Models.ViewModels.Messages;

    public class ChatViewModel
    {
        public ChatViewModel()
        {
            this.Participants = new HashSet<ParticipantViewModel>();
            this.Messages = new HashSet<MessageViewModel>();
        }

        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public string? LastMessage { get; set; }

        public string? LastActive { get; set; }

        public string? CreatedOn { get; set; }

        public ICollection<ParticipantViewModel> Participants { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
