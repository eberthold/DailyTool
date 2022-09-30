namespace DailyTool.DataAccess
{
    internal static class Constants
    {
        public static string JsonStoragePath { get; } = Path.Combine(AppContext.BaseDirectory, "data.json");
    }
}
