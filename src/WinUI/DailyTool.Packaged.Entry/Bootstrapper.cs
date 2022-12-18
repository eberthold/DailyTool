using CommunityToolkit.Mvvm.Messaging;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.DataAccess;
using DailyTool.DataAccess.Framework;
using DailyTool.DataAccess.Generic;
using DailyTool.DataAccess.Meetings;
using DailyTool.DataAccess.People;
using DailyTool.DataAccess.Teams;
using DailyTool.Infrastructure;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.Packaged.Entry.Notifications;
using DailyTool.Packaged.Entry.Threading;
using DailyTool.UserInterface;
using DailyTool.UserInterface.Navigation;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.MeetingInfos;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.Notifications;
using DailyTool.ViewModels.People;
using DailyTool.ViewModels.Settings;
using DailyTool.ViewModels.Teams;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrummy.Core.BusinessLogic.Data;
using Scrummy.Core.BusinessLogic.Teams;
using System;
using System.Data;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;

namespace DailyTool.Packaged.Entry
{
    internal static class Bootstrapper
    {
        private static readonly Type MapperInterfaceDefinition = typeof(IMapper<,>);

        internal static void Bootstrap(this IServiceCollection services)
        {
            services.RegisterNavigation();
            services.RegisterViews();
            services.RegisterViewModels();
            services.RegisterServices();
            services.RegisterRepositories();
            services.RegisterFrameworks();

            services.RegisterSettings();
        }

        private static void RegisterSettings(this IServiceCollection services)
        {
            services.AddTransient<SettingsOverviewViewModel>();

            services.AddTransient<TeamsOverviewViewModel>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
        }

        private static void RegisterNavigation(this IServiceCollection services)
        {
            services.AddSingleton<INavigationMap, NavigationMap>();
            services.AddSingleton<Shell>();
            services.AddSingleton<INavigationService>(provider => provider.GetRequiredService<Shell>());
        }

        private static void RegisterViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        private static void RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<ShellViewModel>();

            services.AddSingleton<IDailyStateProvider, DailyStateProvider>();

            services.AddTransient<InitializationViewModel>();
            services.AddTransient<DailyViewModel>();
            services.AddTransient<AddPersonViewModel>();
            services.AddTransient<MeetingInfoEditViewModel>();
            services.AddTransient<PeopleOverviewViewModel>();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IDailyService, DailyService>();
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IDailyMeetingDataService, DailyMeetingDataService>();
            services.AddSingleton<IParticipantService, ParticipantService>();

            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ITeamService, TeamService>();
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<PersonModel>, GenericRepository<PersonModel, PersonEntity>>();
            services.AddSingleton<IRepository<TeamModel>, GenericRepository<TeamModel, TeamEntity>>();
            services.AddSingleton<IRepository<DailyMeetingModel>, GenericRepository<DailyMeetingModel, DailyMeetingEntityContainer>>();
            services.AddSingleton<IMeetingParticipantsRepository, MeetingParticipantsRepository>();

            services.AddSingleton<IDbContextFactory<DatabaseContext>, DbContextFactory>();
            services.AddSingleton<ITransactionProvider, TransactionProvider>();

            var path = Path.Combine(AppContext.BaseDirectory, "data.db");
            var connectionString = $"DataSource={path}";
            services.AddSqlite<DatabaseContext>(connectionString);
            services.AddSingleton(new SqliteConnection(connectionString));
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

            RegisterMappers(services);
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            Assembly assembly;
            foreach (var assemblyName in assemblyNames.Where(x => x.FullName.Contains("DailyTool")))
            {
                assembly = Assembly.Load(assemblyName);
                foreach (var type in assembly.GetTypes())
                {
                    var interfaces = type.GetInterfaces().Where(CheckIsMapper);
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
    }
}
