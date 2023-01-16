using Scrummy.Core.BusinessLogic.Data;

namespace Scrummy.Core.BusinessLogic.Teams
{
    public interface ITeamService : IDataService<TeamModel>
    {
        Task<int> CreateAsync(TeamModel team);

        Task UpdateAsync(TeamModel team);

        Task DeleteAsync(int id);
    }
}
