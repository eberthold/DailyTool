using CommunityToolkit.Mvvm.Messaging;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.Infrastructure.Abstractions.Data;
using DailyTool.ViewModels.Abstractions;
using Scrummy.Core.BusinessLogic.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.ViewModels.Data
{
    public class OverviewViewModelService<TModel, TViewModel> : IOverviewViewModelService<TViewModel>
        where TModel : IIdentifiable
        where TViewModel : IIdentifiable, INotifyPropertyChanged
    {
        private readonly IDataService<TModel> _dataService;
        private readonly IMapper<TModel, TViewModel> _viewModelMapper;
        private readonly IMessenger _messenger;

        public OverviewViewModelService(
            IDataService<TModel> dataService,
            IMapper<TModel, TViewModel> viewModelMapper,
            IMessenger messenger)
        {
            _dataService = dataService;
            _viewModelMapper = viewModelMapper;
            _messenger = messenger;
        }

        public async Task LoadDataAsync(IOverviewViewModel<TViewModel> viewModel)
        {
            var items = await _dataService.GetAllAsync();
            viewModel.Items = new ObservableCollection<TViewModel>(items.Select(_viewModelMapper.Map));
        }

        public void RegisterItemUpdates(IOverviewViewModel<TViewModel> viewModel)
        {
            _messenger.Register<DataUpdateMessage<TModel>>(viewModel, OnItemChanged);
        }

        public void UnregisterItemUpdates(IOverviewViewModel<TViewModel> viewModel)
        {
            _messenger.Unregister<DataUpdateMessage<TModel>>(viewModel);
        }

        private async void OnItemChanged(object recipient, DataUpdateMessage<TModel> message)
        {
            var viewModel = recipient as IOverviewViewModel<TViewModel>;
            if (viewModel is null)
            {
                return;
            }

            await HandleAddedItems(message, viewModel);
            await HandleUpdatedItems(message, viewModel);
            HandleDeletedItems(message, viewModel);
        }

        private void HandleDeletedItems(DataUpdateMessage<TModel> message, IOverviewViewModel<TViewModel> viewModel)
        {
            foreach (var deletedId in message.Deleted)
            {
                var deletedVm = viewModel.Items.FirstOrDefault(x => x.Id == deletedId);
                if (deletedVm is null)
                {
                    continue;
                }

                viewModel.Items.Remove(deletedVm);
            }
        }

        private async Task HandleUpdatedItems(DataUpdateMessage<TModel> message, IOverviewViewModel<TViewModel> viewModel)
        {
            foreach (var updatedId in message.Updated)
            {
                var existingVm = viewModel.Items.FirstOrDefault(x => x.Id == updatedId);
                if (existingVm is null)
                {
                    continue;
                }

                var updated = await _dataService.GetAsync(updatedId);
                _viewModelMapper.Merge(updated, existingVm);
            }
        }

        private async Task HandleAddedItems(DataUpdateMessage<TModel> message, IOverviewViewModel<TViewModel> viewModel)
        {
            foreach (var addedId in message.Added)
            {
                var added = await _dataService.GetAsync(addedId);
                var addedVm = _viewModelMapper.Map(added);
                viewModel.Items.Add(addedVm);
            }
        }
    }
}
