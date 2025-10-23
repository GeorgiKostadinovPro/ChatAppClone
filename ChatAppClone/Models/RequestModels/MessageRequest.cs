namespace ChatAppClone.Models.RequestModels
{
    using System.ComponentModel.DataAnnotations;

    public class MessageRequest
    {
        [Required(ErrorMessage = "ChatId is required.")]
        public Guid? ChatId { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 500 characters.")]
        public string? Message { get; set; }
    }
}
