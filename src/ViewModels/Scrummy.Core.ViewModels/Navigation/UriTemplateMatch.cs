namespace Scrummy.Core.ViewModels.Navigation
{
    public record UriTemplateMatch
    {
        public Type Type { get; set; } = typeof(object);

        public string UriTemplate { get; init; } = string.Empty;
    }
}
