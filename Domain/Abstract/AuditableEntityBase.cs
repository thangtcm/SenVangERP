namespace Domain.Abstract
{
    public class AuditableEntityBase<T>: EntityBase<T>, IAuditableEntityBase
    {
        #region Managelog
        public required string CreatedBy { get; set; }
        public long CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public long? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public long? DeletedOn { get; set; }
        #endregion
    }
}
