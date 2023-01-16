using DailyTool.BusinessLogic.Daily;
using DailyTool.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using Scrummy.Core.BusinessLogic.Data;
using Scrummy.Core.BusinessLogic.Meeting;

namespace DailyTool.DataAccess.Meetings
{
    public class DailyMeetingRepository : IDailyMeetingRepository
    {
        private readonly IDbContextFactory<ScrummyContext> _dbContextFactory;
        private readonly IMapper<DailyMeetingEntity, DailyMeetingModel> _modelMapper;
        private readonly IRepository<DailyMeetingModel> _repository;

        public DailyMeetingRepository(
            IDbContextFactory<ScrummyContext> dbContextFactory,
            IMapper<DailyMeetingEntity, DailyMeetingModel> modelMapper,
            IRepository<DailyMeetingModel> repository)
        {
            _dbContextFactory = dbContextFactory;
            _modelMapper = modelMapper;
            _repository = repository;
        }

        public Task<int> CreateAsync(DailyMeetingModel model)
            => _repository.CreateAsync(model);

        public Task DeleteAsync(int id)
            => _repository.DeleteAsync(id);

        public Task<IReadOnlyCollection<DailyMeetingModel>> GetAllAsync()
            => _repository.GetAllAsync();

        public async Task<DailyMeetingModel> GetByTeamAsync(int teamId)
        {
            await using var context = await _dbContextFactory
                .CreateDbContextAsync()
                .ConfigureAwait(false);

            var daily = context.Dailies.FirstOrDefault(x => x.TeamId == teamId);
            if (daily is null)
            {
                daily = new DailyMeetingEntity
                {
                    TeamId = teamId,
                    StartTime = TimeSpan.FromHours(9),
                    Duration = TimeSpan.FromMinutes(15)
                };

                context.Dailies.Add(daily);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            return _modelMapper.Map(daily);
        }

        public Task<DailyMeetingModel> GetByIdAsync(int id)
            => _repository.GetByIdAsync(id);

        public Task UpdateAsync(DailyMeetingModel model)
            => _repository.UpdateAsync(model);
    }
}
