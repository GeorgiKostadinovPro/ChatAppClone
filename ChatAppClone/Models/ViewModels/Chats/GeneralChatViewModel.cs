namespace ChatAppClone.Models.ViewModels.Chats
{
    using ChatAppClone.Data.Models;

    public class GeneralChatViewModel
    {
        public GeneralChatViewModel()
        {
            this.Chats = new HashSet<ChatViewModel>();
        }

        public ICollection<ChatViewModel> Chats { get; set; }
    }
}
