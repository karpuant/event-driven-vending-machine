using MediatR;
using VendingMachine.API.Application.Commands.DTO;

namespace VendingMachine.API.Application.Commands
{
    public class PurchaseCommand : IRequest<PurchasetDTO>
    {
        public string ProductName { get; set; }

        public PurchaseCommand(string productName)
        {
            ProductName = productName;
        }

        public override string ToString()
        {
            return $"Command: {this.GetType().Name} [ Product name: {ProductName} ]";
        }
    }
}
