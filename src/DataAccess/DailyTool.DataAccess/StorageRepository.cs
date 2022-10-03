using System.IO.Abstractions;
using System.Text.Json;

namespace DailyTool.DataAccess
{
    public class StorageRepository : IStorageRepository
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

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

        public async Task SaveStorageAsync(Storage storage)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                var content = JsonSerializer.Serialize(storage, _serializerOptions);
                await _fileSystem.File.WriteAllTextAsync(Constants.JsonStoragePath, content).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
