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

        public IEnumerable<Chat> Chats { get; set; }
    }
}
