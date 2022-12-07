using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Framework
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly ITransactionProvider _transactionProvider;
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public DbContextFactory(
            ITransactionProvider transactionProvider,
            IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _transactionProvider = transactionProvider;
            _dbContextFactory = dbContextFactory;
        }

        public DatabaseContext Create()
        {
            var context = _dbContextFactory.CreateDbContext();

            if (_transactionProvider is not null)
            {
                context.Database.UseTransaction(_transactionProvider.CurrentTransaction);
            }

            return context;
        }
    }
}
