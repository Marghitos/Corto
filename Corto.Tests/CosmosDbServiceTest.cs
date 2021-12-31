using Corto.BL.Adapters;
using Corto.BL.Models;
using Corto.BL.Services;
using Corto.Common.DataAccess;
using Corto.Common.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Corto.Tests
{
    [TestClass]
    public class CosmosDbServiceTest
    {
        private readonly int _expiryDaysCount;

        public CosmosDbServiceTest()
        {
            var configuration = Utils.InitConfiguration();
            _expiryDaysCount = int.TryParse(configuration.GetSection("Settings:ExpiryDaysCount").Value, out int result) ? result : 60;
        }

        [TestMethod]
        public void Should_insert_shortened_url()
        {
            var urlItem = new UrlItem
            {
                Id = new Random().Next(0, int.MaxValue).ToString(),
                ShortenedUrl = "ds1450",
                ExpiryDate = DateTime.UtcNow,
                OriginalUrl = "https://www.google.com"
            };

            var cosmosDbRepositoryMock = new Mock<ICosmosDbRepository<UrlItem>>();

            cosmosDbRepositoryMock.Setup(p => p.AddItemAsync(urlItem)).Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Status = HttpStatusCode.OK }));
            cosmosDbRepositoryMock.Setup(p => p.GetItemAsync(urlItem.Id)).Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Status = HttpStatusCode.OK }));

            var cosmosDbResponseGetToUrlManagerServiceResponseAdapter = new CosmosDbResponseGetToUrlManagerServiceResponseAdapter(_expiryDaysCount);
            var cosmosDbResponseInsertToUrlManagerServiceResponseAdapter = new CosmosDbResponseInsertToUrlManagerServiceResponseAdapter();

            var cosmosDbService = new UrlManagerService(cosmosDbRepositoryMock.Object, cosmosDbResponseGetToUrlManagerServiceResponseAdapter, cosmosDbResponseInsertToUrlManagerServiceResponseAdapter);

            var response = cosmosDbService.InsertShortenedUrlAsync(urlItem).GetAwaiter().GetResult();
            Assert.IsTrue(response.ResponseStatus == HttpStatusCode.OK);
        }

        [TestMethod]
        public void Should_get_original_url()
        {
            var urlItem = new UrlItem
            {
                Id = new Random().Next(0, int.MaxValue).ToString(),
                ShortenedUrl = "ds1450",
                ExpiryDate = DateTime.UtcNow,
                OriginalUrl = "https://www.google.com"
            };

            var cosmosDbRepositoryMock = new Mock<ICosmosDbRepository<UrlItem>>();

            cosmosDbRepositoryMock
             .Setup(p => p.AddItemAsync(urlItem))
             .Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Item = urlItem, Status = HttpStatusCode.OK }));

            cosmosDbRepositoryMock
                .Setup(p => p.GetItemAsync(urlItem.Id))
                .Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Item = urlItem, Status = HttpStatusCode.OK }));

            var cosmosDbResponseGetToUrlManagerServiceResponseAdapter = new CosmosDbResponseGetToUrlManagerServiceResponseAdapter(_expiryDaysCount);
            var cosmosDbResponseInsertToUrlManagerServiceResponseAdapter = new CosmosDbResponseInsertToUrlManagerServiceResponseAdapter();

            var cosmosDbService = new UrlManagerService(cosmosDbRepositoryMock.Object, cosmosDbResponseGetToUrlManagerServiceResponseAdapter, cosmosDbResponseInsertToUrlManagerServiceResponseAdapter);

            var response = cosmosDbService.GetOriginalUrlAsync(urlItem.Id).GetAwaiter().GetResult();
            Assert.IsTrue(response.ResponseStatus == HttpStatusCode.OK);
        }

        [TestMethod]
        public void Should_get_expired_shortened_url()
        {
            var urlItem = new UrlItem
            {
                Id = new Random().Next(0, int.MaxValue).ToString(),
                ShortenedUrl = "ds1450",
                ExpiryDate = DateTime.UtcNow.AddDays(-(_expiryDaysCount + 1)),
                OriginalUrl = "https://www.google.com"
            };
            var cosmosDbRepositoryMock = new Mock<ICosmosDbRepository<UrlItem>>();

            cosmosDbRepositoryMock
                .Setup(p => p.AddItemAsync(urlItem))
                .Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Item = urlItem, Status = HttpStatusCode.OK }));

            cosmosDbRepositoryMock
                .Setup(p => p.GetItemAsync(urlItem.Id))
                .Returns(Task.FromResult(new CosmosDbResponse<UrlItem> { Item = urlItem, Status = HttpStatusCode.OK }));

            var cosmosDbResponseGetToUrlManagerServiceResponseAdapter = new CosmosDbResponseGetToUrlManagerServiceResponseAdapter(_expiryDaysCount);
            var cosmosDbResponseInsertToUrlManagerServiceResponseAdapter = new CosmosDbResponseInsertToUrlManagerServiceResponseAdapter();

            var cosmosDbService = new UrlManagerService(cosmosDbRepositoryMock.Object, cosmosDbResponseGetToUrlManagerServiceResponseAdapter, cosmosDbResponseInsertToUrlManagerServiceResponseAdapter);

            var response = cosmosDbService.GetOriginalUrlAsync(urlItem.Id).GetAwaiter().GetResult();

            Assert.IsTrue(response.ResponseStatus == HttpStatusCode.Gone);
        }
    }
}
