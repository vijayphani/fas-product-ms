using Cpa.Fas.ProductMs.Domain.Events;

namespace Cpa.Fas.ProductMs.Domain.Common
{
    public abstract class BaseEntity
    {
        private readonly List<BaseDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(BaseDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
