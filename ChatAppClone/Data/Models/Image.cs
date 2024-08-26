namespace ChatAppClone.Data.Models
{
    using ChatAppClone.Data.Common.Models;

    public class Image : BaseEntityModel<Guid>
    {
        public Image()
        {
            this.Id = Guid.NewGuid();
        }

        public string Url { get; set; } = null!;

        public string PublicId { get; set; } = null!;

        public Guid ChatId { get; set; }

        public virtual Chat Chat { get; set; } = null!;

        public Guid MessageId { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
