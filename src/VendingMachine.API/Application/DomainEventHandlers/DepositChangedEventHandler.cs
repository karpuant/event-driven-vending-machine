using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;
using VendingMachine.Domain.Events;

namespace VendingMachine.API.Application.DomainEventHandlers
{
    public class DepositChangedEventHandler : INotificationHandler<DepositChangedEvent>
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public DepositChangedEventHandler(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        /// <summary>
        /// Updates Read views
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DepositChangedEvent notification, CancellationToken cancellationToken)
        {
            var depo = new DepositDTO { TotalAmount = notification.TotalAmount };
            await _redisCacheClient.Db1.ReplaceAsync<DepositDTO>(ReadViewNames.Deposit, depo);
        }
    }
}
