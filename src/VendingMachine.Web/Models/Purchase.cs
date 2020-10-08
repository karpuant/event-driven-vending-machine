using System.Collections.Generic;

namespace VendingMachine.Web.Models
{
    public class Purchase
    {
        public string ProductName { get; set; }
        public IEnumerable<CoinSet> Change { get; set; }
        public string Message { get; set; }
        public bool Failed { get; set; }
    }

    public class CoinSet
    {
        public int Denomination { get; set; }
        public int Amount { get; set; }
    }
}
