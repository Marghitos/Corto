using Corto.BL.Models;
using Corto.BL.Services;
using Corto.Common.DTO;
using Corto.Common.Interfaces;
using Corto.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NamedServices.Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Corto.API.Controllers
{
    [Route("api/url-shortener")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IKeyRangeService _keyRangeService;
        private readonly IUrlManagerService _urlManagerService;
        private readonly IAlgorithmService _algorithmService;
        private readonly IAdapter<UrlMangerServiceResponse, JsonResult> _urlManagerServiceResponseExpandToApiResponse;
        private readonly IAdapter<UrlMangerServiceResponse, JsonResult> _urlManagerServiceResponseShortenToApiResponse;

        public UrlShortenerController(IServiceProvider serviceProvider)
        {
            _keyRangeService = serviceProvider.GetService<IKeyRangeService>();
            _urlManagerService = serviceProvider.GetService<IUrlManagerService>();
            _algorithmService = serviceProvider.GetService<IAlgorithmService>();
            _urlManagerServiceResponseExpandToApiResponse = serviceProvider.GetNamedService<IAdapter<UrlMangerServiceResponse, JsonResult>>("Expand");
            _urlManagerServiceResponseShortenToApiResponse = serviceProvider.GetNamedService<IAdapter<UrlMangerServiceResponse, JsonResult>>("Shorten");
        }

        [HttpPost]
        [Route("shorten-url")]
        public async Task<IActionResult> ShortenUrl([FromBody] string url)
        {
            if (!UrlUtils.IsUrlValid(url))
                return new JsonResult(url)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            var counter = _keyRangeService.Counter;
            var urlItem = new UrlItem
            {
                Id = counter.ToString(),
                ShortenedUrl = _algorithmService.GenerateShortString(counter),
                OriginalUrl = url,
                ExpiryDate = DateTime.UtcNow
            };
            var response = await _urlManagerService.InsertShortenedUrlAsync(urlItem);

            return _urlManagerServiceResponseShortenToApiResponse.Adapt(response);
        }

        [HttpGet]
        [Route("expand-url")]
        public async Task<IActionResult> ExpandUrl(string url)
        {
            if (!UrlUtils.IsUrlValid(url))
                return new JsonResult(url)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

            var decodedId = _algorithmService.RestoreSeedFromString(url).ToString();
            var response = await _urlManagerService.GetOriginalUrlAsync(decodedId);

            return _urlManagerServiceResponseExpandToApiResponse.Adapt(response);
        }
    }
}