using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.BusinessLogic.Daily
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;

        public PersonService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Task<int> CreatePersonAsync(Person person)
        {
            return _repository.CreatePersonAsync(person);
        }

        public Task DeletePersonAsync(int id)
        {
            return _repository.DeletePersonAsync(id);
        }

        public Task<IReadOnlyCollection<Person>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task UpdatePersonAsync(Person person)
        {
            return _repository.UpdatePersonAsync(person);
        }
    }
}
