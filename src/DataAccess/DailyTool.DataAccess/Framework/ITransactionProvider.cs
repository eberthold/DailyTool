using System.Data.Common;

namespace DailyTool.DataAccess.Framework
{
    public interface ITransactionProvider
    {
        DbTransaction? CurrentTransaction { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
