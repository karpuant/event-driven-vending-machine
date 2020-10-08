using MediatR;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Aggregates.Product;
using VendingMachine.Domain.Aggregates.Wallet;

namespace VendingMachine.Infrastructure
{
    public class VendingMachineUnitOfWork : IUnitOfWork
    {
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly IMediator _mediator;
        public IRepository<Wallet> Wallet { get; }
        public IRepository<Product> Products { get; }

        private ITransaction _transaction;

        public VendingMachineUnitOfWork(IRedisCacheClient redisCacheClient,
                                        IMediator mediator,
                                        IRepository<Wallet> walletRepository, 
                                        IRepository<Product> productRepository)
        {
            _redisCacheClient = redisCacheClient ?? throw new ArgumentNullException(nameof(redisCacheClient));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Wallet = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository)); ;
            Products = productRepository ?? throw new ArgumentNullException(nameof(productRepository)); ;
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _redisCacheClient.Db0.Database.CreateTransaction();
                Wallet.OnTransactionStarted();
                Products.OnTransactionStarted();
            }
            else
                throw new InvalidOperationException("Redis transaction is already created");
        }

        public bool CommitTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Redis transaction is not created");
            var success = _transaction.Execute();

            if (success)
            {
                var changedEntities = new List<Entity>();
                changedEntities.AddRange(Wallet.ChangedEntities);
                changedEntities.AddRange(Products.ChangedEntities);

                _mediator.DispatchDomainEventsAsync(changedEntities).Wait();
            }

            Wallet.OnTransactionCommited();
            Products.OnTransactionCommited();

            return success;
        }
    }
}
