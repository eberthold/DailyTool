using DailyTool.ViewModels.Teams;
using Scrummy.Core.ViewModels.Navigation;

namespace DailyTool.ViewModels.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly INavigationService _navigationService;

        public SettingsProvider(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<IReadOnlyCollection<ISettingsViewModel>> GetSettingsAsync()
        {
            var result = new List<ISettingsViewModel>
            {
                await _navigationService.CreateNavigationTarget<TeamsOverviewViewModel>(),
            };

            return result;
        }
    }
}
