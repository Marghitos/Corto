using Corto.BL.Models;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System;

namespace Corto.BL.Adapters
{
    public class CosmosDbResponseInsertToUrlManagerServiceResponseAdapter : IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>
    {
        public UrlMangerServiceResponse Adapt(CosmosDbResponse<UrlItem> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), $"{nameof(source)} source cannot be null");

            return new UrlMangerServiceResponse
            {
                Url = source.Item?.ShortenedUrl,
                ResponseStatus = source.Status
            };
        }
    }
}
