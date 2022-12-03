using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.Participants
{
    public class ParticpantRepository : IParticipantRepository
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public ParticpantRepository(
            IPersonRepository personRepository,
            IMapper mapper)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            var persons = await _personRepository.GetAllAsync();

            return persons
                .Where(x => x.IsParticipating)
                .Select(_mapper.Map<Participant>)
                .ToList();
        }
    }
}
