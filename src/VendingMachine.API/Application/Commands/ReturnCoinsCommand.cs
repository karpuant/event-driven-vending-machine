using MediatR;
using VendingMachine.API.Application.Commands.DTO;

namespace VendingMachine.API.Application.Commands
{
    public class ReturnCoinsCommand : IRequest<DepositDTO>
    {
        public ReturnCoinsCommand()
        {
        }

        public override string ToString()
        {
            return $"Command: {this.GetType().Name}";
        }
    }
}
