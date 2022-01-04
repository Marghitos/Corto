using System;
using System.Text.RegularExpressions;

namespace Corto.Common.Utils
{
    public static class UrlUtils
    {
        public static bool IsShortenedUrlValid(string url)
        {
            var regex = new Regex(@"^[a-z0-9]*$");
            var match = regex.Match(url);
            return match.Success;
        }

        public static bool IsExpandedUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
