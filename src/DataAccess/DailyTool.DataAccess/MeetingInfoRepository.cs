using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.DataAccess
{
    public class MeetingInfoRepository : IMeetingInfoRepository
    {
        private readonly IStorageRepository<MeetingInfoStorage> _storageRepository;

        public MeetingInfoRepository(
            IStorageRepository<MeetingInfoStorage> storageRepository)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
        }

        public async Task<MeetingInfo> GetAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            return ToBusinessObject(storage);
        }

        public async Task SaveAsync(MeetingInfo meetingInfo)
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            storage.MeetingDuration = meetingInfo.MeetingDuration;
            storage.MeetingStartTime = meetingInfo.MeetingStartTime;
            storage.SprintBoardUri = meetingInfo.SprintBoardUri;

            await _storageRepository.SaveStorageAsync(storage);
        }

        private MeetingInfo ToBusinessObject(MeetingInfoStorage storage)
        {
            return new MeetingInfo
            {
                MeetingDuration = storage.MeetingDuration,
                MeetingStartTime = storage.MeetingStartTime,
                SprintBoardUri = storage.SprintBoardUri,
            };
        }
    }
}
