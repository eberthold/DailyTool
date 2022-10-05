namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IPersonService
    {
        Task<IReadOnlyCollection<Person>> GetAllAsync();

        Task<int> CreatePersonAsync(Person person);

        Task UpdatePersonAsync(Person person);

        Task DeletePersonAsync(int id);
    }
}
