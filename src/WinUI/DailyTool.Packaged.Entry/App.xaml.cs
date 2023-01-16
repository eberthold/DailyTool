using DailyTool.DataAccess;
using DailyTool.UserInterface;
using DailyTool.ViewModels.Dashboard;
using DailyTool.ViewModels.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Navigation;
using System;

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
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
            });
            AppHost = builder.Build();

            InitializeComponent();
        }

        public IHost AppHost { get; private set; }

        private void ConfigureServices(HostBuilderContext hostContext, IServiceCollection serviceCollection)
        {
            serviceCollection.Bootstrap();

            serviceCollection.AddSqlite<ScrummyContext>("Data Source=Scrummy.db");
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            using (var scope = AppHost.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ScrummyContext>();
                var teamService = scope.ServiceProvider.GetRequiredService<ITeamService>();
//#if DEBUG
//                db.Database.EnsureDeleted();
//#endif
                db.Database.Migrate();

//#if DEBUG
//                _ = teamService.CreateAsync(new TeamModel
//                {
//                    Name = "SomeTeam"
//                });
//#endif
            }

            AppHost.Start();

            try
            {
                var window = AppHost.Services.GetRequiredService<MainWindow>();
                var shell = AppHost.Services.GetRequiredService<Shell>();
                shell.Initialize();

                window.Content = shell;
                window.Activate();
                UIHelper.CurrentWindow = window;

                var navigationService = AppHost.Services.GetRequiredService<INavigationService>();
                navigationService.NavigateAsync<DashboardViewModel>();
            }
            catch (Exception ex)
            {
                Exit();
            }
        }
    }
}