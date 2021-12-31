using Corto.BL.Models;
using Corto.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Corto.Tests
{
    [TestClass]
    public class CosmosDbRepositoryTest
    {
        private ICosmosDbRepository<UrlItem> _cosmosDbRepository;

        [TestInitialize]
        public void Initialize()
        {
            var configuration = Utils.InitConfiguration();

            string databaseName = configuration.GetSection("CosmosDb:DatabaseName").Value;
            string containerName = configuration.GetSection("CosmosDb:ContainerName").Value;
            string account = configuration.GetSection("CosmosDb:Account").Value;
            string key = configuration.GetSection("CosmosDb:Key").Value;

            _cosmosDbRepository = new CosmosDbRepository<UrlItem>(databaseName, containerName, account, key);
        }

        [TestMethod]
        public void Should_add_item()
        {
            var urlItem = new UrlItem
            {
                Id = new Random().Next(0, int.MaxValue).ToString(),
                ShortenedUrl = "ds1450",
                ExpiryDate = DateTime.UtcNow,
                OriginalUrl = "https://www.google.com"
            };
            var response = _cosmosDbRepository.AddItemAsync(urlItem).GetAwaiter().GetResult();

            Assert.IsTrue(response.Status == HttpStatusCode.OK);
        }

        [TestMethod]
        public void Should_get_item()
        {
            var urlItem = new UrlItem
            {
                Id = new Random().Next(0, int.MaxValue).ToString(),
                ShortenedUrl = "ds1450",
                ExpiryDate = DateTime.UtcNow,
                OriginalUrl = "https://www.google.com"
            };
            _cosmosDbRepository.AddItemAsync(urlItem).GetAwaiter().GetResult();

            var response = _cosmosDbRepository.GetItemAsync(urlItem.Id).GetAwaiter().GetResult();

            Assert.IsTrue(response.Status == HttpStatusCode.OK);
        }
    }
}
