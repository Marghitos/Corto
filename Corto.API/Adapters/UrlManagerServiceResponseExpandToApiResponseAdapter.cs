using Corto.BL.Models;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Corto.API.Controllers.Adapters
{
    public class UrlManagerServiceResponseExpandToApiResponseAdapter : IAdapter<UrlMangerServiceResponse, JsonResult>
    {
        public JsonResult Adapt(UrlMangerServiceResponse source)
        {
            if (source == null)
                throw new ArgumentNullException("source cannot be null");

            return new JsonResult(source.Url)
            {
                StatusCode = (int)source.ResponseStatus
            };
        }
    }
}
