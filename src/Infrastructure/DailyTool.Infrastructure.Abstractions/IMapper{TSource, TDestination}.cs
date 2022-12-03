namespace DailyTool.Infrastructure.Abstractions;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
}
