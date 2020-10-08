using System.Collections.Generic;

namespace VendingMachine.Web.Models
{
    public class VendingMachineData
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Denomination> Denominations { get; set; }
        public string Deposit { get; set; }
        public Purchase Purchase{ get; set; }

        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public bool HasError => string.IsNullOrEmpty(ErrorMessage);

    }
}
