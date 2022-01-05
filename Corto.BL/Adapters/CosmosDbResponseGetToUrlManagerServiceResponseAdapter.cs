using Corto.BL.Models;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System;
using System.Net;

namespace Corto.BL.Adapters
{
    public class CosmosDbResponseGetToUrlManagerServiceResponseAdapter : IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse>
    {
        private readonly int _expiryDaysCount;

        public CosmosDbResponseGetToUrlManagerServiceResponseAdapter(int expiryDaysCount)
        {
            _expiryDaysCount = expiryDaysCount;
        }

        public UrlMangerServiceResponse Adapt(CosmosDbResponse<UrlItem> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), $"{nameof(source)} source cannot be null");

            return new UrlMangerServiceResponse
            {
                Url = source.Item?.OriginalUrl,
                ResponseStatus = source.Item?.ExpiryDate.AddDays(_expiryDaysCount) < DateTime.UtcNow ? HttpStatusCode.Gone : source.Status
            };
        }
    }
}
