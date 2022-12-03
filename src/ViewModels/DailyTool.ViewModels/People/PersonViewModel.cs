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

        private string _name = string.Empty;
        private bool _isParticipating;

        public PersonViewModel(
            INotificationService notificationService,
            IMapper<PersonViewModel, Person> modelMapper,
            IPersonService personService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _modelMapper = modelMapper ?? throw new ArgumentNullException(nameof(modelMapper));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
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

                _ = UpdateAsync();
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

                _ = UpdateAsync();
            }
        }

        private async Task UpdateAsync()
        {
            var notification = new Notification
            {
                IsRunning = true,
                Text = "TODO: Saving person..."
            };

            await _notificationService.ShowNotificationAsync(notification);

            var model = _modelMapper.Map(this);
            await _personService.UpdatePersonAsync(model);

            notification.IsRunning = false;
            notification.Text = "TODO: Person saved";
        }
    }
}
