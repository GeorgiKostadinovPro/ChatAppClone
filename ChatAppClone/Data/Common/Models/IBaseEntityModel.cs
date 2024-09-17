namespace ChatAppClone.Data.Common.Models
{
    public interface IBaseEntityModel
    {   
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
