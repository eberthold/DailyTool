using System.IO.Abstractions;

namespace DailyTool.DataAccess
{
    public class FileCopy : IFileCopy
    {
        private readonly IFileSystem _fileSystem;

        public FileCopy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task CopyFileAsync(string src, string dest)
        {
            _fileSystem.File.Copy(src, dest, true);
            return Task.CompletedTask;
        }
    }
}
