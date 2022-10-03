using DailyTool.BusinessLogic.People;
using System.IO.Abstractions;

namespace DailyTool.DataAccess
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IStorageRepository<List<PersonStorage>> _storageRepository;

        public PersonRepository(
            IFileSystem fileSystem,
            IStorageRepository<List<PersonStorage>> storageRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
        }

        public async Task<IReadOnlyCollection<Person>> GetAllAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);

            return storage
                .Select(ToBusinessObject)
                .ToList();
        }

        public async Task<IReadOnlyCollection<Person>> GetParticipantsAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            return storage
                .Where(x => x.IsParticipating)
                .Select(ToBusinessObject)
                .ToList();
        }

        public async Task SaveAllAsync(IReadOnlyCollection<Person> people)
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            storage = people.Select(ToStorageObject).ToList();
            await _storageRepository.SaveStorageAsync(storage);
        }

        private Person ToBusinessObject(PersonStorage person)
        {
            return new Person
            {
                Name = person.Name,
                IsParticipating = person.IsParticipating,
            };
        }

        private PersonStorage ToStorageObject(Person person)
        {
            return new PersonStorage
            {
                Name = person.Name,
                IsParticipating = person.IsParticipating,
            };
        }
    }
}
