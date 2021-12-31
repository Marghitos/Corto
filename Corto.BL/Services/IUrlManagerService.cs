using Corto.BL.Models;
using Corto.Common.DTO;
using System.Threading.Tasks;

namespace Corto.BL.Services
{
    public interface IUrlManagerService
    {
        Task<UrlMangerServiceResponse> GetOriginalUrlAsync(string id);
        Task<UrlMangerServiceResponse> InsertShortenedUrlAsync(UrlItem url);
    }
}