using System.Linq;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Events;
using VendingMachine.Domain.Exceptions;
using VendingMachine.Domain.Extensions;

namespace VendingMachine.Domain.Aggregates.Product
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public int Price { get; private set; }
        public int AvailableCount { get; private set; }

        public static string[] SupportedProductNames = new string[]
        {
            "Tea",
            "Espresso",
            "Juice",
            "ChickenSoup"
        };

        public Product(string name, string displayName, int price, int availableCount)
        {
            Name = name;
            DisplayName = displayName;
            Price = price;
            AvailableCount = availableCount;
        }

        public void ValidatePriceAndAvailability(int amountDeposited)
        {
            if (SupportedProductNames.All(n => n != Name))
                throw new VendingMachineDomainException($"Product {DisplayName.ToLower()} not found");
            if (amountDeposited < Price)
                throw new VendingMachineDomainException($"Insufficient amount. Add {(Price - amountDeposited).ToAmounDenominationString()} ");
            if (AvailableCount == 0)
                throw new VendingMachineDomainException($"We run out of {DisplayName.ToLower()}");

        }

        public void CheckOut()
        {
            AvailableCount--;

            if (AvailableCount == 0)
                AddDomainEvent(new ProductRunOutEvent(Name));

            AddDomainEvent(new DepositChangedEvent("Empty"));
        }
    }
}