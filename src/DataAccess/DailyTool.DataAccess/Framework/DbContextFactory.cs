using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Framework
{
    public class DbContextFactory : IDbContextFactory<ScrummyContext>
    {
        private readonly DbContextOptions<ScrummyContext> _options;
        private readonly ITransactionProvider _transactionProvider;

        public DbContextFactory(
            DbContextOptions<ScrummyContext> options,
            ITransactionProvider transactionProvider)
        {
            _options = options;
            _transactionProvider = transactionProvider;
        }

        public ScrummyContext CreateDbContext()
        {
            var context = new ScrummyContext(_options);

            if (_transactionProvider is not null)
            {
                context.Database.UseTransaction(_transactionProvider.CurrentTransaction);
            }

            return context;
        }
    }
}
