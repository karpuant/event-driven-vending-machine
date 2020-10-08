using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Domain.Abstractions;

namespace VendingMachine.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, Entity domainEntity)
        {
            await mediator.DispatchDomainEventsAsync(new List<Entity> { domainEntity });
        }
     
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, IEnumerable<Entity> domainEntities)
        {
            var domainEvents = domainEntities
                .Where(x => x.DomainEvents != null)
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.Publish(domainEvent);
                });

            if (tasks != null &&tasks.Any())
                await Task.WhenAll(tasks);
        }
    }
}
