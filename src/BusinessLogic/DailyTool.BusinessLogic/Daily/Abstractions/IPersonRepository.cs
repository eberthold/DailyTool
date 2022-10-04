namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IPersonRepository
    {
        Task<IReadOnlyCollection<Person>> GetAllAsync();

        Task SaveAllAsync(IReadOnlyCollection<Person> people);

        Task<IReadOnlyCollection<Person>> GetParticipantsAsync();
    }
}
