using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Events;
using VendingMachine.Domain.Exceptions;
using VendingMachine.Domain.Extensions;

namespace VendingMachine.Domain.Aggregates.Wallet
{
    public class Wallet : Entity, IAggregateRoot
    {
        private readonly List<CoinSet> _box;
        public IReadOnlyCollection<CoinSet> Box => _box;

        private readonly List<CoinSet> _deposit;
        public IReadOnlyCollection<CoinSet> Deposit => _deposit;


        public Wallet(List<CoinSet> box, List<CoinSet> deposit)
        {
            _box = box;
            _deposit = deposit;
        }

        public void ValidateAndPutCoinsOnDeposit(int denomination, int amount)
        {
            ValidateDenomination(denomination);

            var newSet = new CoinSet { Denomination = denomination, Amount = amount };

            var existingSet = _deposit.SingleOrDefault(s => s.Denomination == denomination);
            if (existingSet == default)
            {
                _deposit.Add(newSet);
            }
            else
            {
                existingSet.Amount += amount;
            }

            AddDomainEvent(new DepositChangedEvent(_deposit.Sum(c => c.Amount * c.Denomination).ToAmounDenominationString()));
        }

        public IEnumerable<CoinSet> ResetDeposit()
        {
            var resetDeposit = new List<CoinSet>(_deposit);
            
            _deposit.Clear();

            AddDomainEvent(new DepositChangedEvent("Empty"));

            return resetDeposit;
        }


        public IEnumerable<CoinSet> MoveCoinsToWalletAndReturnChange(int price)
        {
            var changeAmount = _deposit.Sum(c => c.TotalAmount) - price;

            AddOrRemoveFromBox(_deposit, false);

            _deposit.Clear();

            var change = CalculateChange(changeAmount);

            // return change from the cashbox
            foreach (var changeSet in change)
            {
                var boxSet = _box.Single(c => c.Denomination == changeSet.Denomination);
                boxSet.Amount -= changeSet.Amount;
            }

            return change;
        }

        private void AddOrRemoveFromBox(List<CoinSet> coinSets, bool isRemove)
        {
            foreach (var s in coinSets)
            {
                var boxSet = _box.Single(c => c.Denomination == s.Denomination);
                boxSet.Amount += isRemove ? -s.Amount : s.Amount;
            }
        }

        private IEnumerable<CoinSet> CalculateChange(int amount)
        {
            var minimumChange = new List<CoinSet>();

            for(var i = _denominations.Length - 1; i >=0; i--)
            {
                var currentDenomination = _denominations[i];
                while (amount >= currentDenomination)
                {
                    amount -= currentDenomination;
                    var coinsOfDenomination = minimumChange.SingleOrDefault(c => c.Denomination == currentDenomination);
                    if (coinsOfDenomination == default)
                    {
                        coinsOfDenomination = new CoinSet { Denomination = currentDenomination, Amount = 0 };
                        minimumChange.Add(coinsOfDenomination);
                    }
                    coinsOfDenomination.Amount++;
                }
            }

            return minimumChange;
        }

        private int[] _denominations = new int[] { 10, 20, 50, 100 };

        private void ValidateDenomination(int denomination)
        {
            if (!_denominations.Contains(denomination))
                throw new VendingMachineDomainException($"{denomination.ToAmounDenominationString()} coins are not accepted");
        }

    }
}
