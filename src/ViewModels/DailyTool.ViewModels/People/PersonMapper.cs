using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.ViewModels.People
{
    public class PersonMapper : IMapper<Person, PersonViewModel>, IMapper<PersonViewModel, Person>
    {
        public PersonViewModel Map(Person source)
        {
            return new PersonViewModel
            {
                Id = source.Id,
                IsParticipating = source.IsParticipating,
                Name = source.Name
            };
        }

        public Person Map(PersonViewModel source)
        {
            return new Person
            {
                Id = source.Id,
                IsParticipating = source.IsParticipating,
                Name = source.Name
            };
        }
    }
}
