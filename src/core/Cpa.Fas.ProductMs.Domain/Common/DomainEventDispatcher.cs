using Cpa.Fas.ProductMs.Domain.Common.Intefaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cpa.Fas.ProductMs.Domain.Common
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IPublisher _publisher;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IPublisher publisher, ILogger<DomainEventDispatcher> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents)
        {
            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToList(); // Get a copy of events
                entity.ClearDomainEvents(); // Clear events from the entity

                foreach (var domainEvent in domainEvents)
                {
                    _logger.LogInformation("Dispatching Domain Event: {DomainEvent}", domainEvent.GetType().Name);
                    await _publisher.Publish(domainEvent);
                }
            }
        }
    }
}
