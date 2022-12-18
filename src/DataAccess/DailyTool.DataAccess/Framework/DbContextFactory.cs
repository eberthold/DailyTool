using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Framework
{
    public class DbContextFactory : IDbContextFactory<DatabaseContext>
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly ITransactionProvider _transactionProvider;

        public DbContextFactory(
            DbContextOptions<DatabaseContext> options,
            ITransactionProvider transactionProvider)
        {
            _options = options;
            _transactionProvider = transactionProvider;
        }

        public DatabaseContext CreateDbContext()
        {
            var context = new DatabaseContext(_options);

            if (_transactionProvider is not null)
            {
                context.Database.UseTransaction(_transactionProvider.CurrentTransaction);
            }

            return context;
        }
    }
}
