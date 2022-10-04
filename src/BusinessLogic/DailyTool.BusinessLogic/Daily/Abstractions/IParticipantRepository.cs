namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantRepository
    {
        Task<IReadOnlyCollection<Participant>> GetAllAsync();
    }
}
