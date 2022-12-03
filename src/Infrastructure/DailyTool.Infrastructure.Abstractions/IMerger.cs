namespace DailyTool.Infrastructure.Abstractions
{
    public interface IMerger<TSource, TDestination>
    {
        void Merge(TDestination destination, TSource source);
    }
}
