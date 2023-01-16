using Scrummy.Core.BusinessLogic.Data;
using Scrummy.Core.BusinessLogic.Meeting;

namespace DailyTool.BusinessLogic.Daily
{
    public interface IDailyMeetingRepository : IRepository<DailyMeetingModel>
    {
        public Task<DailyMeetingModel> GetByTeamAsync(int teamId);
    }
}
