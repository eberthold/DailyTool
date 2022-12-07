using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;

namespace DailyTool.ViewModels.People
{
    public class PersonMapper : IMapper<PersonViewModel, Person>, IMapper<Person, PersonViewModel>
    {
        private readonly INotificationService _notificationService;
        private readonly IPersonService _personService;

        public PersonMapper(
            INotificationService notificationService,
            IPersonService personService)
        {
            _notificationService = notificationService;
            _personService = personService;
        }

        public Person Map(PersonViewModel source)
        {
            var result = new Person();
            Merge(source, result);
            return result;
        }

        public void Merge(PersonViewModel source, Person destination)
        {
            destination.Id = source.Id;
            destination.IsParticipating = source.IsParticipating;
            destination.Name = source.Name;
        }

        public PersonViewModel Map(Person source)
        {
            var result = new PersonViewModel(
                _notificationService,
                this,
                _personService);

            Merge(source, result);
            return result;
        }

        public void Merge(Person source, PersonViewModel destination)
        {
            destination.Id = source.Id;
            destination.IsParticipating = source.IsParticipating;
            destination.Name = source.Name;
        }
    }
}
