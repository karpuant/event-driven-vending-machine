using MediatR;

namespace VendingMachine.Domain.Events
{
    public class DepositChangedEvent : INotification
    {
        public string TotalAmount { get; }

        public DepositChangedEvent(string totalAmount)
        {
            TotalAmount = totalAmount;
        }
    }
}
