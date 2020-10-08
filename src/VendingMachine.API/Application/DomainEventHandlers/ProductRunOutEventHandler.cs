using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.API.Application.Queries.DTO;
using VendingMachine.Domain.Events;

namespace VendingMachine.API.Application.DomainEventHandlers
{
    public class ProductRunOutEventHandler : INotificationHandler<ProductRunOutEvent>
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public ProductRunOutEventHandler(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        /// <summary>
        ///  Updates read views
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(ProductRunOutEvent notification, CancellationToken cancellationToken)
        {
            var products = await _redisCacheClient.Db1.GetAsync<ProductsDTO>(ReadViewNames.Products);
            products.Products.RemoveAll(p => p.Name == notification.ProductName);
            await _redisCacheClient.Db1.ReplaceAsync<ProductsDTO>(ReadViewNames.Products, products);
        }
    }
}
