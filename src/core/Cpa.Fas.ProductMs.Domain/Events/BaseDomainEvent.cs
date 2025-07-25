namespace Cpa.Fas.ProductMs.Domain.Events
{
    public abstract class BaseDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
