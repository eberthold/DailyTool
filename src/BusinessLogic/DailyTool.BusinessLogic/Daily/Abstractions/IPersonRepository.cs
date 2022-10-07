namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IPersonRepository
    {
        Task<int> CreatePersonAsync(Person person);

        Task<IReadOnlyCollection<Person>> GetAllAsync();

        Task<IReadOnlyCollection<Person>> GetAllParticipantsAsync();

        Task UpdatePersonAsync(Person person);

        Task DeletePersonAsync(int id);
    }
}
