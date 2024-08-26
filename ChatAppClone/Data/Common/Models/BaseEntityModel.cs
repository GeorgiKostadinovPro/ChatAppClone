namespace ChatAppClone.Data.Common.Models
{
    public abstract class BaseEntityModel<TKey> : IBaseEntityModel
    {
        public TKey? Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
