namespace Scrummy.Core.BusinessLogic.Teams
{
    public interface ITeamService
    {
        Task<TeamModel> GetAsync(int id);

        Task<IReadOnlyCollection<TeamModel>> GetAllAsync();

        Task<int> CreateAsync(TeamModel team);

        Task UpdateAsync(TeamModel team);

        Task DeleteAsync(int id);
    }
}
