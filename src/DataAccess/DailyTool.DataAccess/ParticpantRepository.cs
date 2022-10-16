using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.DataAccess
{
    public class ParticpantRepository : IParticipantRepository
    {
        private readonly IPersonRepository _personRepository;

        public ParticpantRepository(IPersonRepository personRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            var persons = await _personRepository.GetAllAsync();

            return persons
                .Where(x => x.IsParticipating)
                .Select(person =>
                    new Participant
                    {
                        Id = person.Id,
                        Name = person.Name
                    })
                .ToList();
        }
    }
}
