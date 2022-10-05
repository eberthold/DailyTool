using DailyTool.BusinessLogic.Daily.Abstractions;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;

        public PersonService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Task AddPersonAsync(Person person, DailyState state)
        {
            state.EditablePeople.Add(person);
            return SaveAsync(state.People);
        }

        public async Task LoadAllAsync(DailyState state)
        {
            var people = await _repository.GetAllAsync();
            people = people.OrderBy(x => x.Name).ToList();
            state.EditablePeople = new ObservableCollection<Person>(people);
        }

        public Task RemovePersonAsync(Person person, DailyState state)
        {
            state.EditablePeople.Remove(person);
            return SaveAsync(state.People);
        }

        public Task SaveAsync(IReadOnlyCollection<Person> people)
        {
            return _repository.SaveAllAsync(people);
        }
    }
}
