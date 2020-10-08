using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;

namespace VendingMachine.API.Application.Queries
{
    public interface IDepositQueries
    {
        Task<DepositDTO> GetDepositAsync();
    }
}
