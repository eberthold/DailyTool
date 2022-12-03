namespace DailyTool.DataAccess
{
    public interface IImportable
    {
        Task ImportAsync(string path);
    }
}
