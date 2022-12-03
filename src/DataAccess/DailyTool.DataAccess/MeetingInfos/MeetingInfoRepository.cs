using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.DataAccess.MeetingInfos
{
    public class MeetingInfoRepository : IMeetingInfoRepository, IImportable, IExportable
    {
        private readonly IStorageRepository<MeetingInfoStorage> _storageRepository;
        private readonly IMapper _mapper;
        private readonly IFileCopy _fileCopy;

        public MeetingInfoRepository(
            IStorageRepository<MeetingInfoStorage> storageRepository,
            IMapper mapper,
            IFileCopy fileCopy)
        {
            _storageRepository = storageRepository ?? throw new ArgumentNullException(nameof(storageRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileCopy = fileCopy ?? throw new ArgumentNullException(nameof(fileCopy));
        }

        public async Task<MeetingInfo> GetAsync()
        {
            var storage = await _storageRepository.GetStorageAsync().ConfigureAwait(false);
            return _mapper.Map<MeetingInfo>(storage);
        }

        public async Task SaveAsync(MeetingInfo meetingInfo)
        {
            var storage = _mapper.Map<MeetingInfoStorage>(meetingInfo);

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
    }
}
