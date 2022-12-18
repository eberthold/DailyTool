namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IPersonService
    {
        Task<IReadOnlyCollection<PersonModel>> GetAllAsync();

        Task<int> CreatePersonAsync(PersonModel person);

        Task UpdatePersonAsync(PersonModel person);

        Task DeletePersonAsync(int id);
    }
}
