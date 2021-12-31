using System.Net;

namespace Corto.Common.DTO
{
    public class UrlMangerServiceResponse
    {
        public string Url { get; set; }
        public HttpStatusCode ResponseStatus { get; set; }
    }
}
