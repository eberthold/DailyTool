using DailyTool.Infrastructure.Abstractions.Data;

namespace Scrummy.Core.BusinessLogic.Data
{
    public interface IRepository<T>
        where T : IIdentifiable
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<int> CreateAsync(T model);

        Task UpdateAsync(T model);

        Task DeleteAsync(int id);
    }
}
