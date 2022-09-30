namespace DailyTool.DataAccess
{
    public interface IStorageRepository
    {
        public Task<Storage> GetStorageAsync();

        public Task SaveStorageAsync(Storage storage);
    }
}
