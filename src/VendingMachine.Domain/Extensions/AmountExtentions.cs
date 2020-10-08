namespace VendingMachine.Domain.Extensions
{
    public static class AmountExtentions
    {
        public static string ToAmounDenominationString(this int amount)
            => amount >= 100 ? $"{(double)amount / 100:0.##} euro" : $"{amount:0.} cents";
    }
}
