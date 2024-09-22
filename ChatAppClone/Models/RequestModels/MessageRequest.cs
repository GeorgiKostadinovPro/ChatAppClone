namespace ChatAppClone.Models.RequestModels
{
    public class MessageRequest
    {
        public Guid ChatId { get; set; }

        public string Message { get; set; }
    }
}
