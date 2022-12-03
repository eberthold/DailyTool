namespace DailyTool.DataAccess
{
    public interface IExportable
    {
        Task ExportAsync(string path);
    }
}
