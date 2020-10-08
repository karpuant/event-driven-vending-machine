using VendingMachine.Domain.Aggregates.Wallet;

namespace VendingMachine.API.Application.Commands.DTO
{
    public class CoinSetDTO
    {
        public int Denomination { get; set; }
        public int Amount { get; set; }

        public static CoinSetDTO FromCoinSet(CoinSet coinSet)
            => new CoinSetDTO
            {
                Denomination = coinSet.Denomination,
                Amount = coinSet.Amount
            };
    }
}