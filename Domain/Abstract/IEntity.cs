namespace Domain.Abstract
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}