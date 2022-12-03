using CommunityToolkit.Mvvm.Messaging;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.DataAccess;
using DailyTool.DataAccess.MeetingInfos;
using DailyTool.DataAccess.Participants;
using DailyTool.DataAccess.People;
using DailyTool.Infrastructure;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.Packaged.Entry.Navigation;
using DailyTool.Packaged.Entry.Notifications;
using DailyTool.Packaged.Entry.Threading;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.MeetingInfos;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.Notifications;
using DailyTool.ViewModels.People;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;

namespace DailyTool.Packaged.Entry
{
    internal static class Bootstrapper
    {
        private static readonly Type MapperInterfaceDefinition = typeof(IMapper<,>);
        private static readonly Type MergerInterfaceDefinition = typeof(IMerger<,>);

        internal static void Bootstrap(this IServiceCollection services)
        {
            services.RegisterNavigation();
            services.RegisterViews();
            services.RegisterViewModels();
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
        }

        private static void RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<IDailyStateProvider, DailyStateProvider>();

            services.AddTransient<InitializationViewModel>();
            services.AddTransient<DailyViewModel>();
            services.AddTransient<AddPersonViewModel>();
            services.AddTransient<MeetingInfoEditViewModel>();
            services.AddTransient<PeopleOverviewViewModel>();

            services.AddSingleton<IPersonViewModelFactory, PersonViewModelFactory>();
            services.AddTransient<MainWindowViewModel>();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDailyService, DailyService>();
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IMeetingInfoService, MeetingInfoService>();
            services.AddSingleton<IParticipantService, ParticipantService>();

            services.AddSingleton<INotificationService, NotificationService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IMeetingInfoRepository, MeetingInfoRepository>();
            services.AddSingleton<IStorageRepository<MeetingInfoStorage>, StorageRepository<MeetingInfoStorage>>();
            services.AddSingleton<IStorageRepository<List<PersonStorage>>, StorageRepository<List<PersonStorage>>>();
            services.AddSingleton<IParticipantRepository, ParticpantRepository>();

            services.AddSingleton<IFileCopy, FileCopy>();
        }

        private static void RegisterFrameworks(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
            services.AddSingleton<ITimestampProvider, TimeStampProvider>();
            services.AddSingleton<IRandomProvider, RandomProvider>();
            services.AddSingleton<IMainThreadInvoker, MainThreadInvoker>();
            services.AddSingleton<IMapper, GenericMapper>();
            services.AddSingleton<ITaskQueue, TaskQueue>();

            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            Assembly assembly;
            foreach (var assemblyName in assemblyNames.Where(x => x.FullName.Contains("DailyTool")))
            {
                assembly = Assembly.Load(assemblyName);
                foreach (var type in assembly.GetTypes())
                {
                    var interfaces = type.GetInterfaces().Where(x => CheckIsMapper(x) || CheckIsMerger(x));
                    foreach (var @interface in interfaces)
                    {
                        services.AddSingleton(@interface, type);
                    }
                }
            }
        }

        private static bool CheckIsMapper(Type x)
        {
            if (!x.IsGenericType)
            {
                return false;
            }

            return x.GetGenericTypeDefinition() == MapperInterfaceDefinition;
        }

        private static bool CheckIsMerger(Type x)
        {
            if (!x.IsGenericType)
            {
                return false;
            }

            return x.GetGenericTypeDefinition() == MergerInterfaceDefinition;
        }
    }
}
