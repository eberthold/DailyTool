using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DailyTool.DataAccess
{
    internal class DatabaseContextDesignTimeFactory : IDesignTimeDbContextFactory<ScrummyContext>
    {
        public ScrummyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScrummyContext>();
            optionsBuilder.UseSqlite("Data Source=:memory:");

            return new ScrummyContext(optionsBuilder.Options);
        }
    }
}
