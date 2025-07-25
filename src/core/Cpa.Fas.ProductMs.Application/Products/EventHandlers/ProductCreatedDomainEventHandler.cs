using Cpa.Fas.ProductMs.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cpa.Fas.ProductMs.Application.Products.EventHandlers
{
    public class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly ILogger<ProductCreatedDomainEventHandler> _logger;

        public ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProductMs Domain Event: {DomainEvent}", notification.GetType().Name);

            // Here you would implement actual business logic related to the event,
            // e.g., send an email, update a search index, log to a specific system.
            _logger.LogInformation("Product created: Id={ProductId}, Name={ProductName}, Price={Price}, Stock={Stock}",
                notification.ProductId, notification.ProductName, notification.Price, notification.Stock);

            return Task.CompletedTask;
        }
    }
}

