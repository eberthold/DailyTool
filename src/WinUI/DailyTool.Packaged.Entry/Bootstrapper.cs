using CommunityToolkit.Mvvm.Messaging;
using DailyTool.BusinessLogic.Initialization;
using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.People;
using DailyTool.DataAccess;
using DailyTool.Packaged.Entry.Navigation;
using DailyTool.UserInterface.Initialization;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace DailyTool.Packaged.Entry
{
    internal static class Bootstrapper
    {
        internal static void Bootstrap(this IServiceCollection services)
        {
            services.RegisterNavigation();
            services.RegisterViews();
            services.RegisterViewModels();
            services.RegisterControllers();
            services.RegisterServices();
            services.RegisterRepositories();
            services.RegisterFrameworks();
        }

        private static void RegisterNavigation(this IServiceCollection services)
        {
            services.AddSingleton<INavigationMap, NavigationMap>();
            services.AddSingleton<INavigationService>(provider => provider.GetRequiredService<MainWindow>());
        }

        private static void RegisterViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<InitializationView>();
        }

        private static void RegisterViewModels(this IServiceCollection services)
        {
            services.AddTransient<InitializationViewModel>();
            services.AddTransient<AddPersonViewModel>();
        }

        private static void RegisterControllers(this IServiceCollection services)
        {
            services.AddSingleton<IInitializationStateController, InitializationStateController>();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IInitializationService, InitializationService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IMeetingInfoRepository, MeetingInfoRepository>();
            services.AddSingleton<IStorageRepository, StorageRepository>();
        }

        private static void RegisterFrameworks(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        }
    }
}
