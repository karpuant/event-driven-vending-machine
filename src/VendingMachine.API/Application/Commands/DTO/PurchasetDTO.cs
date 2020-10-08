using System.Collections.Generic;

namespace VendingMachine.API.Application.Commands.DTO
{
    public class PurchasetDTO
    {
        public string ProductName { get; set; }
        public IEnumerable<CoinSetDTO> Change { get; set; }
        public string Message { get; set; }
        public bool Failed { get; set; }
    }
}