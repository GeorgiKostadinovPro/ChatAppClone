namespace ChatAppClone.Data.Common.Models
{
    public interface IDeletableModel
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
