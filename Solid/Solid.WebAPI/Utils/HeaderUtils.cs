namespace Solid.WebAPI.Utils
{
    public class HeaderUtils
    {
        public static void ValidateHeaders(IHeaderDictionary headers, string[] requiredHeaders)
        {
            if (requiredHeaders.Any(h => !headers.Any()))
            {
                throw new BadHttpRequestException(string.Empty);
            }
        }
    }
}
