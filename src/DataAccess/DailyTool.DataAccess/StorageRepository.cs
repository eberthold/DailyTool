using System.IO.Abstractions;
using System.Text.Json;

namespace DailyTool.DataAccess
{
    public class StorageRepository<T> : IStorageRepository<T>
        where T : new()
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly IFileSystem _fileSystem;
        private readonly string _storagePath;

        public StorageRepository(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _storagePath = Constants.StoragePaths[typeof(T)];
        }

        public async Task<T> GetStorageAsync()
        {
            if (!_fileSystem.File.Exists(_storagePath))
            {
                return new T();
            }

            var content = await _fileSystem.File.ReadAllTextAsync(_storagePath).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(content) ?? new();
        }

        public async Task SaveStorageAsync(T storage)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                var content = JsonSerializer.Serialize(storage, _serializerOptions);
                await _fileSystem.File.WriteAllTextAsync(_storagePath, content).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
