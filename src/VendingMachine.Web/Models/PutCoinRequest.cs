using System.Collections.Generic;

namespace VendingMachine.Web.Models
{
    public class PutCoinRequest
    {
        public int Denomination { get; set; }
        public int Amount { get; set; }
    }
}
