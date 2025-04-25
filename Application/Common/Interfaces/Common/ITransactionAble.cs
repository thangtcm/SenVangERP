namespace Application.Common.Interfaces.Common
{
    public interface ITransactionAble
    {
        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Applies the outstanding operations in the current transaction to the database.
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Discards the outstanding operations in the current transaction.
        /// </summary>
        void RollbackTransaction();
        /// <summary>
        /// Dispose Transaction
        /// </summary>
        void DisposeTransaction();
    }
}
