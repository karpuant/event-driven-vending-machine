using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;

namespace VendingMachine.API.Application.Queries
{
    public class DepositQueries : IDepositQueries
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public DepositQueries(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public async Task<DepositDTO> GetDepositAsync()
        {
            return await _redisCacheClient.Db1.GetAsync<DepositDTO>(ReadViewNames.Deposit) ?? new DepositDTO() ;
        }
    }
}
