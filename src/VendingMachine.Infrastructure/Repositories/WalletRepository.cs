using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Aggregates.Wallet;

namespace VendingMachine.Infrastructure.Repositories
{
    public class WalletRepository : IRepository<Wallet>
    {
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly IMediator _mediator;
        private bool _isTransaction = false;

        public List<Wallet> ChangedEntities { get; private set; } = new List<Wallet>();

        public WalletRepository(IRedisCacheClient redisCacheClient,
                                IMediator mediator)
        {
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Wallet> FindByIdAsync(string id)
        {
            return await _redisCacheClient.Db0.GetAsync<Wallet>(id);
        }

        public Task<IEnumerable<Wallet>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Wallet entity)
        {
            if (_isTransaction)
            {
                ChangedEntities.Add(entity);
            }
            else
            {
                await _mediator.DispatchDomainEventAsync(entity);
            }
            return await _redisCacheClient.Db0.ReplaceAsync<Wallet>("wallet", entity);
        }


        public void OnTransactionStarted()
        {
            _isTransaction = true;
        }

        public void OnTransactionCommited()
        {
            _isTransaction = false;
            ChangedEntities = new List<Wallet>();
        }
    }
}
