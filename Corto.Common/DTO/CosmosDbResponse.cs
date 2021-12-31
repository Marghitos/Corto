using System.Net;

namespace Corto.Common.DTO
{
    public class CosmosDbResponse<T> where T : class
    {
        public HttpStatusCode Status { get; set; }
        public T Item { get; set; }
    }
}
