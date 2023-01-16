using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Tests
{
    internal class InMemoryDbContextFactory : IDbContextFactory<ScrummyContext>
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly string _connectionString;
        private readonly DbContextOptions<ScrummyContext> _options;

        private SqliteConnection? _connection;

        public InMemoryDbContextFactory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScrummyContext>();
            _connectionString = $"Data Source={_guid};mode=memory;cache=shared";
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            _options = optionsBuilder
                .UseSqlite(_connection)
                .Options;
        }

        public ScrummyContext CreateDbContext()
        {
            var context = new ScrummyContext(_options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
