using DailyTool.BusinessLogic.Parameters;

namespace DailyTool.DataAccess
{
    public class MeetingInfoRepository : IMeetingInfoRepository
    {
        private readonly IStorageRepository _storageRepository;

        public MeetingInfoRepository(
            IStorageRepository storageRepository)
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

        private MeetingInfo ToBusinessObject(Storage storage)
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
