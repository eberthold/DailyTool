using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace DailyTool.DataAccess.Framework
{
    public class TransactionProvider : ITransactionProvider
    {
        private readonly SqliteConnection _sqliteConnection;

        public TransactionProvider(SqliteConnection sqliteConnection)
        {
            _sqliteConnection = sqliteConnection;
        }

        public DbTransaction? CurrentTransaction { get; set; }

        public void BeginTransaction()
        {
            if (CurrentTransaction is not null)
            {
                throw new InvalidOperationException("Only one transaction allowed at given time");
            }

            CurrentTransaction = _sqliteConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (CurrentTransaction is null)
            {
                throw new InvalidOperationException("No transaction exists to commit");
            }

            CurrentTransaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (CurrentTransaction is null)
            {
                throw new InvalidOperationException("No transaction exists to rollback");
            }

            CurrentTransaction.Rollback();
        }
    }
}
