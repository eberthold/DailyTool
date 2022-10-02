namespace DailyTool.BusinessLogic.People
{
    public interface IPersonRepository
    {
        Task<IReadOnlyCollection<Person>> GetAllAsync();

        Task SaveAllAsync(IReadOnlyCollection<Person> people);

        Task<IReadOnlyCollection<Person>> GetParticipantsAsync();
    }
}
