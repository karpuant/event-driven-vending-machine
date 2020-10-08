using MediatR;

namespace VendingMachine.Domain.Events
{
    public class ProductRunOutEvent : INotification
    {
        public string ProductName { get; }

        public ProductRunOutEvent(string productName)
        {
            ProductName = productName;
        }
    }
}
