namespace Scrummy.Core.ViewModels.Navigation
{
    public class NavigationTargetAttribute : Attribute
    {
        public NavigationTargetAttribute(string relativeUri)
        {
            RelativeUriTemplate = relativeUri;
        }

        public string RelativeUriTemplate { get; }
    }
}
