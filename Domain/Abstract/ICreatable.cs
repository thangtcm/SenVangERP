namespace Domain.Abstract
{
    public interface ICreatable
    {
        string CreatedBy { get; set; }
        long CreatedOn { get; set; }
    }
}
