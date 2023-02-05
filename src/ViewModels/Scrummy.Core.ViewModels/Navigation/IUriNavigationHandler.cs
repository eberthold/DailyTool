using System.Diagnostics.CodeAnalysis;

namespace Scrummy.Core.ViewModels.Navigation
{
    public interface IUriNavigationHandler
    {
        string GetParameterlessUriOf<T>() 
            where T : class, INavigationTarget;

        string GetMatchingUriOf<T, TParameter>()
            where T : class, INavigationTarget<TParameter>;

        /// <summary>
        /// Gets the navigation target for the given uri string
        /// </summary>
        Task<UriTemplateMatch> GetTargetTypeForUri(string uri);

        /// <summary>
        /// Tries to exactly match the given uri parameters to one of the expected navigation parameter types.
        /// </summary>
        bool TryMatchParameters<T>(string uriTemplate, string uri, [NotNullWhen(true)] out T? parameter)
            where T : class, new();
    }
}
