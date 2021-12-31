using Corto.BL.Models;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Corto.API.Controllers.Adapters
{
    public class UrlManagerServiceResponseShortenToApiResponseAdapter : IAdapter<UrlMangerServiceResponse, ApiResponse>
    {
        private readonly string _baseUrl;

        public UrlManagerServiceResponseShortenToApiResponseAdapter(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public ApiResponse Adapt(UrlMangerServiceResponse source)
        {
            if (source == null)
                throw new ArgumentNullException("source cannot be null");

            return new ApiResponse
            {
                HttpStatusCode = source.ResponseStatus,
                Url = $"{_baseUrl}{source.Url}"
            };
        }
    }
}
