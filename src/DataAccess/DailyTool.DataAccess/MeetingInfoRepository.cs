using DailyTool.BusinessLogic.Parameters;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyTool.DataAccess
{
    public class MeetingInfoRepository : IMeetingInfoRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IStorageRepository _storageRepository;

        public MeetingInfoRepository(
            IFileSystem fileSystem,
            IStorageRepository storageRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
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

            await _storageRepository.SaveStorageAsync(storage);
        }

        private MeetingInfo ToBusinessObject(Storage storage)
        {
            return new MeetingInfo
            {
                MeetingDuration = storage.MeetingDuration,
                MeetingStartTime = storage.MeetingStartTime
            };
        }
    }
}
