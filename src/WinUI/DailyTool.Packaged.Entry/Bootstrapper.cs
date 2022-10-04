using CommunityToolkit.Mvvm.Messaging;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;
using DailyTool.DataAccess;
using DailyTool.Packaged.Entry.Navigation;
using DailyTool.Packaged.Entry.Threading;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
            services.RegisterServices();
            services.RegisterRepositories();
            services.RegisterStates();
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
        }

        private static void RegisterViewModels(this IServiceCollection services)
        {
            services.AddTransient<InitializationViewModel>();
            services.AddTransient<DailyViewModel>();
            services.AddTransient<AddPersonViewModel>();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDailyService, DailyService>();
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IParticipantService, ParticipantService>();
            services.AddSingleton<IMeetingInfoService, MeetingInfoService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IMeetingInfoRepository, MeetingInfoRepository>();
            services.AddSingleton<IStorageRepository<MeetingInfoStorage>, StorageRepository<MeetingInfoStorage>>();
            services.AddSingleton<IStorageRepository<List<PersonStorage>>, StorageRepository<List<PersonStorage>>>();
            services.AddSingleton<IParticipantRepository, ParticipantRepository>();
        }

        private static void RegisterStates(this IServiceCollection services)
        {
            services.AddSingleton<DailyState>();
        }

        private static void RegisterFrameworks(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
            services.AddSingleton<ITimeStampProvider, TimeStampProvider>();
            services.AddSingleton<IMainThreadInvoker, MainThreadInvoker>();
        }
    }
}
