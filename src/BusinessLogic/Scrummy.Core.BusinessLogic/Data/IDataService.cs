using DailyTool.Infrastructure.Abstractions.Data;

namespace Scrummy.Core.BusinessLogic.Data
{
    public interface IDataService<T>
        where T : IIdentifiable
    {
        Task<T> GetAsync(int id);

        Task<IReadOnlyCollection<T>> GetAllAsync();
    }
}
