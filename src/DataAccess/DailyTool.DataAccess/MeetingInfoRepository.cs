using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.DataAccess
{
    public class MeetingInfoRepository : IMeetingInfoRepository, IImportable, IExportable
    {
        private readonly IStorageRepository<MeetingInfoStorage> _storageRepository;
        private readonly IFileCopy _fileCopy;

        public MeetingInfoRepository(
            IStorageRepository<MeetingInfoStorage> storageRepository,
            IFileCopy fileCopy)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
            _fileCopy = fileCopy ?? throw new ArgumentNullException(nameof(fileCopy));
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

        public Task ImportAsync(string path)
        {
            return _fileCopy.CopyFileAsync(path, Constants.StoragePaths[typeof(MeetingInfoStorage)]);
        }

        public Task ExportAsync(string path)
        {
            return _fileCopy.CopyFileAsync(Constants.StoragePaths[typeof(MeetingInfoStorage)], path);
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
