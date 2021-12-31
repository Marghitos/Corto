namespace Corto.Common.Utils
{
    public static class UrlUtils
    {
        public static bool IsUrlValid(string url)
        {
            return !string.IsNullOrWhiteSpace(url);
        }
    }
}
