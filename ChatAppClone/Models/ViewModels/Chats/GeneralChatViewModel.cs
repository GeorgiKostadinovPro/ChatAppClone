namespace ChatAppClone.Models.ViewModels.Chats
{
    using ChatAppClone.Data.Models;

    public class GeneralChatViewModel
    {
        public GeneralChatViewModel()
        {
            this.Chats = new HashSet<Chat>();
        }

        public Chat? Chat { get; set; }

        public ICollection<Chat> Chats { get; set; }
    }
}
