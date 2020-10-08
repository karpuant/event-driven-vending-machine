namespace VendingMachine.Domain.Aggregates.Wallet
{
    public class CoinSet
    {
        public int Denomination { get; set; }
        public int Amount { get; set; }

        public int TotalAmount => Denomination * Amount;
    }
}