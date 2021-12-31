using Corto.BL.Models;
using Corto.Common.DataAccess;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using System.Threading.Tasks;

namespace Corto.BL.Services
{
    public class UrlManagerService : IUrlManagerService
    {
        private readonly ICosmosDbRepository<UrlItem> _cosmosDbRepository;
        private readonly IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse> _cosmosDbResponseGetToUrlManagerServiceResponseAdapter;
        private readonly IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse> _cosmosDbResponseInsertToUrlManagerServiceResponseAdapter;

        public UrlManagerService(
            ICosmosDbRepository<UrlItem> cosmosDbRepository,
            IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse> cosmosDbResponseGetToUrlManagerServiceResponseAdapter,
            IAdapter<CosmosDbResponse<UrlItem>, UrlMangerServiceResponse> cosmosDbResponseInsertToUrlManagerServiceResponseAdapter
            )
        {
            _cosmosDbRepository = cosmosDbRepository;
            _cosmosDbResponseGetToUrlManagerServiceResponseAdapter = cosmosDbResponseGetToUrlManagerServiceResponseAdapter;
            _cosmosDbResponseInsertToUrlManagerServiceResponseAdapter = cosmosDbResponseInsertToUrlManagerServiceResponseAdapter;
        }

        public async Task<UrlMangerServiceResponse> InsertShortenedUrlAsync(UrlItem url)
        {
            var response = await _cosmosDbRepository.AddItemAsync(url);
            return _cosmosDbResponseInsertToUrlManagerServiceResponseAdapter.Adapt(response);
        }

        public async Task<UrlMangerServiceResponse> GetOriginalUrlAsync(string id)
        {
            var response = await _cosmosDbRepository.GetItemAsync(id);
            return _cosmosDbResponseGetToUrlManagerServiceResponseAdapter.Adapt(response);
        }
    }
}
