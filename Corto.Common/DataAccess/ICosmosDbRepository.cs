using Corto.Common.DTO;
using System.Threading.Tasks;

namespace Corto.Common.DataAccess
{
    public interface ICosmosDbRepository<T> where T : class
    {
        Task<CosmosDbResponse<T>> AddItemAsync(T item);
        Task<CosmosDbResponse<T>> GetItemAsync(string id);
    }
}