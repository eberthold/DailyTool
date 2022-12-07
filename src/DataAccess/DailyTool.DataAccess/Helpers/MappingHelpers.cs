namespace DailyTool.DataAccess.Helpers
{
    internal static class MappingHelpers
    {
        /// <summary>
        /// Maps the string and makes it <see langword="null"/> if its empty.
        /// </summary>
        public static string? MapToEntity(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            return source;
        }

        /// <summary>
        /// Maps the string and ensures that it isn't <see langword="null"/>.
        /// </summary>
        public static string MapToModel(this string? source)
        {
            return source ?? string.Empty;
        }
    }
}
