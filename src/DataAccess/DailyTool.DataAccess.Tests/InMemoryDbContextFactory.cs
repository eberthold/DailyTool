using DailyTool.DataAccess.Framework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Tests
{
    internal class InMemoryDbContextFactory : IDbContextFactory
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly string _connectionString;
        private readonly DbContextOptions<DatabaseContext> _options;

        private SqliteConnection? _connection;

        public InMemoryDbContextFactory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            _connectionString = $"DataSource=:memory:";
            _connection = new SqliteConnection(_connectionString);
            _options = optionsBuilder.UseSqlite(_connection).Options;
        }

        public DatabaseContext Create()
        {
            var context = new DatabaseContext(_options);
            context.Database.Migrate();
            return context;
        }
    }
}
