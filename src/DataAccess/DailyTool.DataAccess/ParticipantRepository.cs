using DailyTool.BusinessLogic.Daily;

namespace DailyTool.DataAccess
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IStorageRepository _storageRepository;

        public ParticipantRepository(
            IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
        }

        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            var participants = storage
                .People
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
