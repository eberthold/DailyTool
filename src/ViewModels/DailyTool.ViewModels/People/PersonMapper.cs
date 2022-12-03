using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.People
{
    public class PersonMapper : IMapper<PersonViewModel, Person>, IMerger<Person, PersonViewModel>
    {
        public Person Map(PersonViewModel source)
        {
            return new Person
            {
                Id = source.Id,
                IsParticipating = source.IsParticipating,
                Name = source.Name
            };
        }

        public void Merge(PersonViewModel destination, Person source)
        {
            destination.Id = source.Id;
            destination.IsParticipating = source.IsParticipating;
            destination.Name = source.Name;
        }
    }
}
