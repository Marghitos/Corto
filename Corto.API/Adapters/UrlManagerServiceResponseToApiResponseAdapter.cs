using Corto.BL.Models;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System;

namespace Corto.API.Controllers.Adapters
{
    public class UrlManagerServiceResponseToApiResponseAdapter : IAdapter<UrlMangerServiceResponse, ApiResponse>
    {
        public ApiResponse Adapt(UrlMangerServiceResponse source)
        {
            if (source == null)
                throw new ArgumentNullException("source cannot be null");

            return new ApiResponse
            {
                HttpStatusCode = source.ResponseStatus,
                Url = source.Url
            };
        }
    }
}
