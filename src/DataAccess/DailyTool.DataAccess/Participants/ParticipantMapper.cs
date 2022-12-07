using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.Participants
{
    public class ParticipantMapper : IMapper<Person, Participant>
    {
        public Participant Map(Person source)
        {
            return new Participant
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public void Merge(Person source, Participant destination)
        {
            throw new NotImplementedException();
        }
    }
}
