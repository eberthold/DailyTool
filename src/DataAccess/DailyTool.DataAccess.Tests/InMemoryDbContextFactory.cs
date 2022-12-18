using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Tests
{
    internal class InMemoryDbContextFactory : IDbContextFactory<DatabaseContext>
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly string _connectionString;
        private readonly DbContextOptions<DatabaseContext> _options;

        private SqliteConnection? _connection;

        public InMemoryDbContextFactory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            _connectionString = $"Data Source={_guid};mode=memory;cache=shared";
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            _options = optionsBuilder
                .UseSqlite(_connection)
                .Options;
        }

        public DatabaseContext CreateDbContext()
        {
            var context = new DatabaseContext(_options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
