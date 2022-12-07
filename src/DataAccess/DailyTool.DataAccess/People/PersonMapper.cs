using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.People
{
    public class PersonMapper : IMapper<PersonStorage, Person>, IMapper<Person, PersonStorage>
    {
        public PersonStorage Map(Person source)
        {
            return new PersonStorage
            {
                Id = source.Id,
                Name = source.Name,
                IsParticipating = source.IsParticipating
            };
        }

        public Person Map(PersonStorage source)
        {
            return new Person
            {
                Id = source.Id,
                Name = source.Name,
                IsParticipating = source.IsParticipating
            };
        }

        public void Merge(Person source, PersonStorage destination)
        {
            throw new NotImplementedException();
        }

        public void Merge(PersonStorage source, Person destination)
        {
            throw new NotImplementedException();
        }
    }
}
