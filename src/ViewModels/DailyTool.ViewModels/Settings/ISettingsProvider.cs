namespace DailyTool.ViewModels.Settings
{
    public interface ISettingsProvider
    {
        Task<IReadOnlyCollection<ISettingsViewModel>> GetSettingsAsync();
    }
}
