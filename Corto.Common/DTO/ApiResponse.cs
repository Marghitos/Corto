using System.Net;

namespace Corto.Common.DTO
{
    public class ApiResponse
    {
        public string Url { get; set; }
        public HttpStatusCode HttpStatusCode {get;set;}
    }
}
