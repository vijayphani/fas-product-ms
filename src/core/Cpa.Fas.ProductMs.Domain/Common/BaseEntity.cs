using Cpa.Fas.ProductMs.Domain.Common.Intefaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Fas.ProductMs.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        private readonly List<BaseDomainEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public Guid Id { get; set; }

        protected void AddDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
