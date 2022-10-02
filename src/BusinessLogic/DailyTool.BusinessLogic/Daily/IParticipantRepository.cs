using DailyTool.BusinessLogic.Parameters;

namespace DailyTool.BusinessLogic.Daily
{
    public interface IParticipantRepository
    {
        Task<IReadOnlyCollection<Participant>> GetAllAsync();
    }
}
