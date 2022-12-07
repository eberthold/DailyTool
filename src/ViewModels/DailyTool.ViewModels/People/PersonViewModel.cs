using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;

namespace DailyTool.ViewModels.People
{
    public class PersonViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper<PersonViewModel, Person> _modelMapper;
        private readonly IPersonService _personService;
        private readonly ITaskQueue _taskQueue;

        private string _name = string.Empty;
        private bool _isParticipating;

        public PersonViewModel(
            INotificationService notificationService,
            IMapper<PersonViewModel, Person> modelMapper,
            IPersonService personService,
            ITaskQueue taskQueue)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _modelMapper = modelMapper ?? throw new ArgumentNullException(nameof(modelMapper));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _taskQueue = taskQueue;
        }

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (!SetProperty(ref _name, value))
                {
                    return;
                }

                _taskQueue.Enqueue(UpdateAsync);
            }
        }

        public bool IsParticipating
        {
            get => _isParticipating;
            set
            {
                if (!SetProperty(ref _isParticipating, value))
                {
                    return;
                }

                _taskQueue.Enqueue(UpdateAsync);
            }
        }

        private async Task UpdateAsync()
        {
            var notification = new Notification
            {
                IsRunning = true,
                Text = "TODO: Saving person..."
            };

            try
            {
                await _notificationService.ShowNotificationAsync(notification);

                var model = _modelMapper.Map(this);
                await _personService.UpdatePersonAsync(model);

                notification.IsRunning = false;
                notification.Text = "TODO: Person saved";
            }
            finally
            {
                notification.IsRunning = false;
                notification.Text = "TODO: failed to save person";
            }
        }
    }
}
