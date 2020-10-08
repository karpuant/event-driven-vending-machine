using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Aggregates.Product;

namespace VendingMachine.Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly IMediator _mediator;
        private bool _isTransaction = false;

        public List<Product> ChangedEntities { get; private set; } = new List<Product>();

        public ProductRepository(IRedisCacheClient redisCacheClient,
                                 IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
        }

        public async Task<Product> FindByIdAsync(string id)
        {
            return await _redisCacheClient.Db0.GetAsync<Product>(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _redisCacheClient.Db0.GetAllAsync<Product>(Product.SupportedProductNames);
            return products?.Values ?? new List<Product>();
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            if (_isTransaction)
            {
                ChangedEntities.Add(entity);
            }
            else
            {
                await _mediator.DispatchDomainEventAsync(entity);
            }
            return await _redisCacheClient.Db0.ReplaceAsync<Product>(entity.Name, entity);
        }

        public void OnTransactionStarted()
        {
            _isTransaction = true;
        }

        public void OnTransactionCommited()
        {
            _isTransaction = false;
            ChangedEntities = new List<Product>();
        }
    }
}
