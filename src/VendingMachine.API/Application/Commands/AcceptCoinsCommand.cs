using MediatR;
using VendingMachine.API.Application.Commands.DTO;

namespace VendingMachine.API.Application.Commands
{
    public class AcceptCoinsCommand : IRequest<DepositDTO>
    {
        public int Denomination { get; set; }
        public int Amount { get; set; }

        public AcceptCoinsCommand(int denomination, int amount)
        {
            Denomination = denomination;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Command: {this.GetType().Name} [ Denomination: {Denomination} cents, Amount: {Amount} coins ]";
        }
    }
}
