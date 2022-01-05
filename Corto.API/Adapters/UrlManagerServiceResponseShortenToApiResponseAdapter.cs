using Corto.Common.DTO;
using Corto.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Corto.API.Controllers.Adapters
{
    public class UrlManagerServiceResponseShortenToApiResponseAdapter : IAdapter<UrlMangerServiceResponse, JsonResult>
    {
        private readonly string _baseUrl;

        public UrlManagerServiceResponseShortenToApiResponseAdapter(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public JsonResult Adapt(UrlMangerServiceResponse source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source),$"{nameof(source)} cannot be null");

            return new JsonResult($"{_baseUrl}{source.Url}")
            {
                StatusCode = (int)source.ResponseStatus
            };
        }
    }
}
