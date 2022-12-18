using DailyTool.Infrastructure.Abstractions.Data;
using System.Collections.Immutable;

namespace Scrummy.Core.BusinessLogic.Data
{
    public class DataUpdateMessage<T>
        where T : IIdentifiable
    {
        public IReadOnlyCollection<int> Added { get; init; } = ImmutableArray<int>.Empty;

        public IReadOnlyCollection<int> Updated { get; init; } = ImmutableArray<int>.Empty;

        public IReadOnlyCollection<int> Deleted { get; init; } = ImmutableArray<int>.Empty;

        public static DataUpdateMessage<T> FromAdded(params int[] added)
        {
            return new DataUpdateMessage<T>
            {
                Added = added
            };
        }

        public static DataUpdateMessage<T> FromUpdated(params int[] updated)
        {
            return new DataUpdateMessage<T>
            {
                Updated = updated
            };
        }

        public static DataUpdateMessage<T> FromDeleted(params int[] deleted)
        {
            return new DataUpdateMessage<T>
            {
                Deleted = deleted
            };
        }
    }
}
