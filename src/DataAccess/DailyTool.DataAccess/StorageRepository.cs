using System.IO.Abstractions;
using System.Text.Json;

namespace DailyTool.DataAccess
{
    public class StorageRepository : IStorageRepository
    {
        private readonly IFileSystem _fileSystem;

        public StorageRepository(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public async Task<Storage> GetStorageAsync()
        {
            if (!_fileSystem.File.Exists(Constants.JsonStoragePath))
            {
                return new Storage();
            }

            var content = await _fileSystem.File.ReadAllTextAsync(Constants.JsonStoragePath).ConfigureAwait(false);
            return JsonSerializer.Deserialize<Storage>(content) ?? new();
        }

        public Task SaveStorageAsync(Storage storage)
        {
            var content = JsonSerializer.Serialize(storage);
            return _fileSystem.File.WriteAllTextAsync(Constants.JsonStoragePath, content);
        }
    }
}
