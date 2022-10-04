using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.DataAccess
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IStorageRepository<List<PersonStorage>> _storageRepository;

        public ParticipantRepository(
            IStorageRepository<List<PersonStorage>> storageRepository)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
        }

        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            var participants = storage
                .Where(x => x.IsParticipating)
                .Select(ToBusinessObject)
                .ToList();

            return participants;
        }

        private static Participant ToBusinessObject(PersonStorage personStorage)
        {
            return new Participant
            {
                Name = personStorage.Name
            };
        }
    }
}
