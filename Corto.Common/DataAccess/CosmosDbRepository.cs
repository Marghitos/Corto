using Corto.Common.DTO;
using Microsoft.Azure.Cosmos;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Corto.Common.DataAccess
{
    public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : class
    {
        private readonly string _databaseName;
        private readonly string _containerName;
        private readonly string _account;
        private readonly string _key;

        private Container _container;

        public CosmosDbRepository(string databaseName, string containerName, string account, string key)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException("databaseName cannot be null or empty");
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentNullException("containerName cannot be null or empty");
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentNullException("account cannot be null or empty");
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key cannot be null or empty");

            _databaseName = databaseName;
            _containerName = containerName;
            _account = account;
            _key = key;

            Initialize().GetAwaiter().GetResult();
        }

        private async Task Initialize()
        {
            var client = new CosmosClient(_account, _key);
            var database = await client.CreateDatabaseIfNotExistsAsync(_databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(_containerName, "/id");
            _container = client.GetContainer(_databaseName, _containerName);
        }

        public async Task<CosmosDbResponse<T>> AddItemAsync(T item)
        {
            try
            {
                var type = item.GetType();
                var propertyInfo = type.GetProperty("Id");

                var response = await _container.CreateItemAsync<T>(item, new PartitionKey(propertyInfo.GetValue(item).ToString()));
                return new CosmosDbResponse<T>
                {
                    Item = response,
                    Status = HttpStatusCode.OK
                };
            }
            catch (CosmosException)
            {
                return new CosmosDbResponse<T>
                {
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<CosmosDbResponse<T>> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return new CosmosDbResponse<T>
                {
                    Item = response.Resource,
                    Status = HttpStatusCode.OK
                };
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return new CosmosDbResponse<T>
                {
                    Status = HttpStatusCode.NotFound
                };
            }
        }
    }
}
