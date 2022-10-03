namespace DailyTool.DataAccess
{
    public interface IStorageRepository<T>
        where T : new()
    {
        public Task<T> GetStorageAsync();

        public Task SaveStorageAsync(T storage);
    }
}
