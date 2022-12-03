namespace DailyTool.Infrastructure.Abstractions
{
    public interface IMapper
    {
        TDestination Map<TDestination>(object source);
    }
}
