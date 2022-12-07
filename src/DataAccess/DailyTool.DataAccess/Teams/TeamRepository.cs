using DailyTool.DataAccess.Framework;
using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Data;
using Scrummy.Core.BusinessLogic.Exceptions;
using Scrummy.Core.BusinessLogic.Teams;

namespace DailyTool.DataAccess.Teams
{
    public class TeamRepository : IRepository<TeamModel>
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper<TeamModel, TeamEntity> _entityMapper;
        private readonly IMapper<TeamEntity, TeamModel> _modelMapper;

        public TeamRepository(
            IDbContextFactory dbContextFactory,
            IMapper<TeamModel, TeamEntity> entityMapper,
            IMapper<TeamEntity, TeamModel> modelMapper)
        {
            _dbContextFactory = dbContextFactory;
            _entityMapper = entityMapper;
            _modelMapper = modelMapper;
        }

        public async Task<int> CreateAsync(TeamModel model)
        {
            await using var dbContext = _dbContextFactory.Create();

            var team = _entityMapper.Map(model);
            team.Id = 0;

            dbContext.Teams.Add(team);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return team.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await using var dbContext = _dbContextFactory.Create();

            var team = dbContext.Teams.FindAsync(id).ConfigureAwait(false);
            dbContext.Remove(team);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<TeamModel>> GetAllAsync()
        {
            await using var dbContext = _dbContextFactory.Create();

            var teams = dbContext.Teams;
            return teams.Select(_modelMapper.Map).ToList();
        }

        public async Task<TeamModel> GetByIdAsync(int id)
        {
            await using var dbContext = _dbContextFactory.Create();

            var team = dbContext.Teams.Find(id);
            if (team is null)
            {
                throw new NotFoundException<TeamModel>();
            }

            return _modelMapper.Map(team);
        }

        public async Task UpdateAsync(TeamModel model)
        {
            await using var dbContext = _dbContextFactory.Create();

            var team = dbContext.Teams.Find(model.Id);
            if (team is null)
            {
                throw new NotFoundException<TeamModel>();
            }

            _entityMapper.Merge(model, team);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
