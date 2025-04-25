namespace Domain.Abstract
{
    public interface IModifiable
    {
        string? ModifiedBy { get; set; }
        long? ModifiedOn { get; set; }
    }
}
