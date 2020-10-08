using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Aggregates.Wallet;
using VendingMachine.Domain.Extensions;

namespace VendingMachine.API.Application.Commands.DTO
{
    public class DepositDTO
    {
        public IEnumerable<CoinSetDTO> CoinSets { get; set; }
        public string TotalAmount { get; set; }

        public static DepositDTO FromCoinSets(IEnumerable<CoinSet> coinSets)
            => new DepositDTO
            {
                CoinSets = coinSets.Select(c => new CoinSetDTO { Denomination = c.Denomination, Amount = c.Amount }),
                TotalAmount = coinSets.Sum(c => (c.Amount * c.Denomination)).ToAmounDenominationString()
            };
    }
}