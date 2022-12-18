using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;

namespace DailyTool.ViewModels.People
{
    public class PersonMapper : IMapper<PersonViewModel, PersonModel>, IMapper<PersonModel, PersonViewModel>
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

        public PersonModel Map(PersonViewModel source)
        {
            var result = new PersonModel();
            Merge(source, result);
            return result;
        }

        public void Merge(PersonViewModel source, PersonModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }

        public PersonViewModel Map(PersonModel source)
        {
            var result = new PersonViewModel(
                _notificationService,
                this,
                _personService);

            Merge(source, result);
            return result;
        }

        public void Merge(PersonModel source, PersonViewModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }
    }
}
