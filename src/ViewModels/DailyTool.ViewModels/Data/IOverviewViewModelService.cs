using DailyTool.Infrastructure.Abstractions.Data;
using DailyTool.ViewModels.Abstractions;
using System.ComponentModel;

namespace DailyTool.ViewModels.Data
{
    public interface IOverviewViewModelService<TViewModel>
        where TViewModel : IIdentifiable, INotifyPropertyChanged
    {
        Task LoadDataAsync(IOverviewViewModel<TViewModel> viewModel);

        void RegisterItemUpdates(IOverviewViewModel<TViewModel> viewModel);

        void UnregisterItemUpdates(IOverviewViewModel<TViewModel> viewModel);
    }
}