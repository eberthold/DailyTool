namespace DailyTool.ViewModels.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public SettingsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<IReadOnlyCollection<ISettingsViewModel>> GetSettingsAsync()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var allTypes = assemblies.SelectMany(x => x.GetTypes());
            var settingsViewModelTypes = allTypes.Where(x => x.GetInterfaces().Contains(typeof(ISettingsViewModel)));

            var instances = settingsViewModelTypes
                .Select(x => _serviceProvider.GetService(x))
                .OfType<ISettingsViewModel>()
                .ToList();

            return Task.FromResult<IReadOnlyCollection<ISettingsViewModel>>(instances);
        }
    }
}
