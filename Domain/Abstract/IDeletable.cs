namespace Domain.Abstract
{
    public interface IDeletable
    {
        public bool IsDeleted { get; set; }
        string? DeletedBy { get; set; }
        long? DeletedOn { get; set; }
    }
}
