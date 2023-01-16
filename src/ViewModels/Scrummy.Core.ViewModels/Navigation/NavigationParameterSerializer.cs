using System.Reflection;
using System.Text;
using System.Web;

namespace Scrummy.Core.ViewModels.Navigation
{
    public static class NavigationParameterSerializer
    {
        public static string Serialize(object parameter)
        {
            var type = parameter.GetType();
            var properties = type
                .GetProperties()
                .OrderBy(x => x.Name)
                .ToList();

            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                var value = property.GetValue(parameter);
                if (value is null)
                {
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.Append("&");
                }

                builder.Append($"{property.Name}={value.ToString()}");
            }

            return builder.ToString();
        }

        public static T Deserialize<T>(string parameter)
            where T : new()
        {
            var type = typeof(T);
            var properties = type
                .GetProperties()
                .OrderBy(x => x.Name)
                .ToList();

            var queryString = HttpUtility.ParseQueryString(parameter);
            var result = new T();
            foreach (var property in properties)
            {
                if (property is null)
                {
                    continue;
                }

                var value = queryString.Get(property.Name);
                if (value is null)
                {
                    continue;
                }

                var parsedValue = TryParse(property, value);
                property.SetValue(result, parsedValue);
            }

            return result;
        }

        private static object TryParse(PropertyInfo property, string value)
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
    }
}
