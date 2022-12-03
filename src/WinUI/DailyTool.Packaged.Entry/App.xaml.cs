using DailyTool.UserInterface;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace DailyTool.Packaged.Entry
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var services = new ServiceCollection();
            services.Bootstrap();

            var options = new ServiceProviderOptions
            {
                ValidateOnBuild = true
            };
            var serviceProvider = services.BuildServiceProvider(options);
            UIHelper.ServiceProvider = serviceProvider;

            var window = serviceProvider.GetRequiredService<MainWindow>();
            var windowViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
            window.DataContext = windowViewModel;
            window.Activate();
            UIHelper.CurrentWindow = window;

            var navigationService = serviceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateAsync<InitializationViewModel>();
        }
    }
}