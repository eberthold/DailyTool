using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantFactory : IParticipantFactory<ParticipantViewModel>
    {
        public ParticipantViewModel Create(Person person)
        {
            return new ParticipantViewModel
            {
                Id = person.Id,
                Name = person.Name,
            };
        }
    }
}
