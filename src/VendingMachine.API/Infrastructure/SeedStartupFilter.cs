using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;
using VendingMachine.Domain.Aggregates.Product;
using VendingMachine.Domain.Aggregates.Wallet;
using VendingMachine.Domain.Extensions;

namespace VendingMachine.API.Infrastructure
{
    public class SeedStartupFilter : IStartupFilter
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public SeedStartupFilter(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                Task.WaitAll(ApplyOnStorage(), ApplyOnReadViews());
                next(builder);
            };
        }

        public async Task ApplyOnStorage()
        {
            await _redisCacheClient.Db0.ReplaceAsync("wallet", _initialWallet);

            foreach (var product in _initialProducts)
            {
                await _redisCacheClient.Db0.ReplaceAsync(product.Name, product);
            }
        }

        public async Task ApplyOnReadViews()
        {
            await _redisCacheClient.Db1.ReplaceAsync<DepositDTO>(ReadViewNames.Deposit, new DepositDTO { TotalAmount = "0 cents" });

            var products = new ProductsDTO
            {
                Products = _initialProducts.Select(p => new ProductDTO
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Price = p.Price.ToAmounDenominationString()
                }).ToList()
            };

            await _redisCacheClient.Db1.ReplaceAsync<ProductsDTO>(ReadViewNames.Products, products);
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


        static class ReadViewNames
        {
            public static string Deposit = "Deposit";
            public static string Products = "Products";
        }
    }
}
