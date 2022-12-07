using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;

namespace DailyTool.ViewModels.People
{
    public class PersonViewModelFactory : IPersonViewModelFactory
    {
        private readonly IMerger<Person, PersonViewModel> _viewModelMerger;
        private readonly IMapper<PersonViewModel, Person> _modelMapper;
        private readonly INotificationService _notificationService;
        private readonly IPersonService _personService;
        private readonly ITaskQueue _taskQueue;

        public PersonViewModelFactory(
            IMerger<Person, PersonViewModel> viewModelMerger,
            IMapper<PersonViewModel, Person> modelMapper,
            INotificationService notificationService,
            IPersonService personService,
            ITaskQueue taskQueue)
        {
            _viewModelMerger = viewModelMerger ?? throw new ArgumentNullException(nameof(viewModelMerger));
            _modelMapper = modelMapper ?? throw new ArgumentNullException(nameof(modelMapper));
            _notificationService = notificationService;
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _taskQueue = taskQueue;
        }

        public PersonViewModel Create(Person person)
        {
            var result = new PersonViewModel(
                _notificationService,
                _modelMapper,
                _personService,
                _taskQueue);

            _viewModelMerger.Merge(result, person);

            return result;
        }
    }
}
