namespace DailyTool.BusinessLogic.Peoples
{
    public interface IPersonRepository
    {
        Task<IReadOnlyCollection<Person>> GetAllAsync();

        Task SaveAllAsync(IReadOnlyCollection<Person> peoples);

        Task<IReadOnlyCollection<Person>> GetParticipantsAsync();
    }
}
