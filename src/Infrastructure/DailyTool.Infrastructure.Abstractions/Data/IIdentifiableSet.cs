namespace DailyTool.Infrastructure.Abstractions.Data
{
    public interface IIdentifiableSet : IIdentifiable
    {
        new int Id { get; set; }
    }
}
