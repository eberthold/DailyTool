using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.Infrastructure
{
    public class GenericMapper : IMapper
    {
        private readonly IServiceProvider _serviceProvider;

        public GenericMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TDestination Map<TDestination>(object source)
        {
            var sourceType = source.GetType();
            var mapperBaseType = typeof(IMapper<,>);
            var mapperType = mapperBaseType.MakeGenericType(new[] { sourceType, typeof(TDestination) });

            var mapper = _serviceProvider.GetService(mapperType);
            if (mapper is null)
            {
                throw new InvalidOperationException($"No mapper found for mapping from {sourceType.FullName} to {typeof(TDestination).FullName}");
            }

            var mapMethod = mapperType.GetMethod(nameof(IMapper<object, object>.Map));
            return (TDestination)mapMethod!.Invoke(mapper, new[] { source })!;
        }
    }
}
