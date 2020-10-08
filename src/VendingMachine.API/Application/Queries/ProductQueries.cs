using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;

namespace VendingMachine.API.Application.Queries
{
    public class ProductQueries : IProductQueries
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public ProductQueries(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public async Task<ProductsDTO> GetAvailableProductsAsync()
        {
            return await _redisCacheClient.Db1.GetAsync<ProductsDTO>(ReadViewNames.Products) ?? new ProductsDTO();
        }
    }
}
