using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using VendingMachine.Domain.Aggregates.Product;
using VendingMachine.Domain.Aggregates.Wallet;

namespace VendingMachine.Infrastructure
{
    public class Seed
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public Seed(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
        }


        public void ApplyOnStorage()
        {

        }

        public void ApplyOnReadViews()
        {

        }

        private Wallet _initialWallet = new Wallet(
            new List<CoinSet>
            {
                new CoinSet
                {
                    Denomination = 10,
                    Amount = 100
                },

                new CoinSet
                {
                    Denomination = 20,
                    Amount = 100
                },

                new CoinSet
                {
                    Denomination = 50,
                    Amount = 100
                },

                new CoinSet
                {
                    Denomination = 100,
                    Amount = 100
                }
            },
            new List<CoinSet>()
        );

        private List<Product> _initialProducts = new List<Product>
        {
           new Product ("Tea", "Tea", 130, 10),
           new Product ("Espresso", "Espresso", 180, 20),
           new Product ("Juice", "Juice", 180, 20),
           new Product ("ChickenSoup", "Chicken soup", 180, 15)
        };
    }
}
