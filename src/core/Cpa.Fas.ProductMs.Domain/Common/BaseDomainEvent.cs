namespace Cpa.Fas.ProductMs.Domain.Common
{
    public abstract class BaseDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
