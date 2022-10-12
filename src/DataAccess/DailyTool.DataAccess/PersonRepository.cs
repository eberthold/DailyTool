using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.DataAccess
{
    public class PersonRepository : IPersonRepository, IImportable, IExportable
    {
        private readonly IStorageRepository<List<PersonStorage>> _storageRepository;
        private readonly IFileCopy _fileCopy;

        public PersonRepository(
            IStorageRepository<List<PersonStorage>> storageRepository,
            IFileCopy fileCopy)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
            _fileCopy = fileCopy ?? throw new ArgumentNullException(nameof(fileCopy));
        }

        public async Task<int> CreatePersonAsync(Person person)
        {
            var people = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            var id = people.Max(x => x.Id) + 1;
            var storagePerson = ToStorageObject(person);
            storagePerson.Id = id;
            people.Add(storagePerson);
            await _storageRepository.SaveStorageAsync(people).ConfigureAwait(false);
            return id;
        }

        public async Task DeletePersonAsync(int id)
        {
            var people = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            var person = people.FirstOrDefault(x => x.Id == id);
            if (person is null)
            {
                return;
            }

            people.Remove(person);
            await _storageRepository.SaveStorageAsync(people).ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<Person>> GetAllAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);

            return storage
                .Select(ToBusinessObject)
                .ToList();
        }

        public async Task<IReadOnlyCollection<Person>> GetAllParticipantsAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);

            return storage
                .Where(x => x.IsParticipating)
                .Select(ToBusinessObject)
                .ToList();
        }

        public Task ImportAsync(string path)
        {
            return _fileCopy.CopyFileAsync(path, Constants.StoragePaths[typeof(List<PersonStorage>)]);
        }

        public Task ExportAsync(string path)
        {
            return _fileCopy.CopyFileAsync(Constants.StoragePaths[typeof(List<PersonStorage>)], path);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            var people = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            var storagePerson = people.FirstOrDefault(x => x.Id == person.Id);
            if (storagePerson is null)
            {
                return;
            }

            storagePerson.Name = person.Name;
            storagePerson.IsParticipating = person.IsParticipating;
            await _storageRepository.SaveStorageAsync(people);
        }

        private Person ToBusinessObject(PersonStorage person)
        {
            return new Person
            {
                Id = person.Id,
                Name = person.Name,
                IsParticipating = person.IsParticipating,
            };
        }

        private PersonStorage ToStorageObject(Person person)
        {
            return new PersonStorage
            {
                Id = person.Id,
                Name = person.Name,
                IsParticipating = person.IsParticipating,
            };
        }
    }
}
