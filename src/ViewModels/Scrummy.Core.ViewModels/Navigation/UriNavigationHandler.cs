using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Linq;

namespace Scrummy.Core.ViewModels.Navigation
{
    /// <remarks>
    /// This solution works as long as we won't use blazor wasm.
    /// </remarks>
    public class UriNavigationHandler : IUriNavigationHandler
    {
        private IReadOnlyDictionary<string, Type> _uriTypeMap = new Dictionary<string, Type>();
        private readonly ParameterNameComparer _parameterNameComparer = new ParameterNameComparer();

        public UriNavigationHandler()
        {

        }

        public Task<UriTemplateMatch> GetTargetTypeForUri(string uri)
        {
            EnsureUriTypeMap();

            foreach (var uriMap in _uriTypeMap)
            {
                var template = TemplateParser.Parse(uriMap.Key);
                var values = new RouteValueDictionary();
                var matcher = new TemplateMatcher(template, values);
                var isMatch = matcher.TryMatch(uri, values);

                if (isMatch)
                {
                    return Task.FromResult(new UriTemplateMatch
                    {
                        Type = uriMap.Value,
                        UriTemplate = uriMap.Key
                    });
                }
            }

            throw new NavigationException($"unable to find target for {uri}");
        }

        public bool TryMatchParameters<T>(string uriTemplate, string uri, [NotNullWhen(true)] out T? parameter)
            where T : class, new()
        {
            parameter = null;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            var template = TemplateParser.Parse(uriTemplate);
            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);
            var isMatch = matcher.TryMatch(uri, values);
            if (!isMatch)
            {
                return false;
            }

            var differentProperties = values
                .Keys
                .Except(
                    properties.Select(x => x.Name), 
                    _parameterNameComparer)
                .Any();
            if (differentProperties)
            {
                return false;
            }

            parameter = new T();
            foreach (var property in properties)
            {
                var value = values[property.Name];

                var parsedValue = ParseRouteValue(property, value as string ?? string.Empty);
                property.SetValue(parameter, parsedValue);
            }

            return true;
        }

        private static object ParseRouteValue(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(string))
            {
                return value;
            }
            else if (property.PropertyType == typeof(bool))
            {
                return bool.Parse(value);
            }
            else if (property.PropertyType == typeof(int))
            {
                return int.Parse(value);
            }
            else if (property.PropertyType == typeof(long))
            {
                return int.Parse(value);
            }
            else if (property.PropertyType == typeof(float))
            {
                return float.Parse(value);
            }
            else if (property.PropertyType == typeof(double))
            {
                return double.Parse(value);
            }

            throw new InvalidOperationException($"unable to parse {value} to {property.PropertyType}");
        }

        private void EnsureUriTypeMap()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(x => x.DefinedTypes);

            var map = new Dictionary<string, Type>();
            foreach (var type in allTypes)
            {
                var navigationAttributes = type.GetCustomAttributes(true).OfType<NavigationTargetAttribute>();
                foreach (var navigationAttribute in navigationAttributes)
                {
                    map.Add(navigationAttribute.RelativeUriTemplate, type);
                }
            }

            _uriTypeMap = map;
        }

        public string GetParameterlessUriOf<T>()
            where T : class, INavigationTarget
        {
            var targetType = typeof(T);
            var navigationAttributes = targetType.GetCustomAttributes(true).OfType<NavigationTargetAttribute>();

            string? navigationUri = null;
            foreach (var attribute in navigationAttributes)
            {
                var template = TemplateParser.Parse(attribute.RelativeUriTemplate);

                // we are navigating parameterless in this case
                if (template.Parameters.Count > 0)
                {
                    continue;
                }

                navigationUri = attribute.RelativeUriTemplate;
            }

            if (navigationUri is null)
            {
                throw new NavigationException($"{typeof(T)} has no parameterless navigation route defined");
            }

            return navigationUri;
        }

        public string GetMatchingUriOf<T, TParameter>()
            where T : class, INavigationTarget<TParameter>
        {
            var targetType = typeof(T);
            var navigationAttributes = targetType.GetCustomAttributes(true).OfType<NavigationTargetAttribute>();

            string? navigationUri = null;
            var parameterType = typeof(TParameter);
            var expectedParameters = parameterType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                .ToList();
            foreach (var attribute in navigationAttributes)
            {
                var template = TemplateParser.Parse(attribute.RelativeUriTemplate);

                if (template.Parameters.Count != expectedParameters.Count)
                {
                    continue;
                }

                var parameterMismatch = template
                    .Parameters
                    .Select(x => x.Name)
                    .Except(expectedParameters.Select(x => x.Name), _parameterNameComparer)
                    .Any();
                if (parameterMismatch)
                {
                    continue;
                }

                navigationUri = attribute.RelativeUriTemplate;
            }

            if (navigationUri is null)
            {
                throw new NavigationException($"{typeof(T)} has no parameterless navigation route defined");
            }

            return navigationUri;
        }

        private class ParameterNameComparer : EqualityComparer<string>
        {
            /// <inheritdoc />
            public override bool Equals(string? x, string? y)
            {
                if (x is null || y is null)
                {
                    return false;
                }

                return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
            }

            /// <inheritdoc />
            public override int GetHashCode([DisallowNull] string obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
        }
    }
}
