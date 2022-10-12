namespace DailyTool.DataAccess
{
    public interface IFileCopy
    {
        Task CopyFileAsync(string src, string dest);
    }
}
