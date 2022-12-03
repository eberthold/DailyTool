using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.Infrastructure
{
    public class RandomProvider : IRandomProvider
    {
        public Random GetRandom()
            => new Random();
    }
}
