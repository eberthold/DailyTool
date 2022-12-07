using CommunityToolkit.Mvvm.Messaging;
using Scrummy.Core.BusinessLogic.Data;

namespace Scrummy.Core.BusinessLogic.Teams
{
    public class TeamService : ITeamService
    {
        private readonly IRepository<TeamModel> _teamRepository;
        private readonly IMessenger _messenger;

        public TeamService(
            IRepository<TeamModel> teamRepository,
            IMessenger messenger)
        {
            _teamRepository = teamRepository;
            _messenger = messenger;
        }

        public async Task<int> CreateAsync(TeamModel team)
        {
            var result = await _teamRepository.CreateAsync(team);

            _messenger.Send(DataUpdateMessage<TeamModel>.FromAdded(result));

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _teamRepository.DeleteAsync(id);

            _messenger.Send(DataUpdateMessage<TeamModel>.FromDeleted(id));
        }

        public Task<IReadOnlyCollection<TeamModel>> GetAllAsync()
        {
            return _teamRepository.GetAllAsync();
        }

        public Task<TeamModel> GetAsync(int id)
        {
            return _teamRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(TeamModel team)
        {
            await _teamRepository.UpdateAsync(team);
            _messenger.Send(DataUpdateMessage<TeamModel>.FromUpdated(team.Id));
        }
    }
}
