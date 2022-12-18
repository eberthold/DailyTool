using DailyTool.BusinessLogic.Daily.Abstractions;
using Scrummy.Core.BusinessLogic.Data;

namespace DailyTool.BusinessLogic.Daily
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<PersonModel> _repository;

        public PersonService(IRepository<PersonModel> repository)
        {
            _repository = repository;
        }

        public Task<int> CreatePersonAsync(PersonModel person)
        {
            return _repository.CreateAsync(person);
        }

        public Task DeletePersonAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IReadOnlyCollection<PersonModel>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task UpdatePersonAsync(PersonModel person)
        {
            return _repository.UpdateAsync(person);
        }
    }
}
