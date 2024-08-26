namespace ChatAppClone.Data.Common.Models
{
    public class IBaseEntityModel
    {
        Guid Id { get; set; }
        
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
