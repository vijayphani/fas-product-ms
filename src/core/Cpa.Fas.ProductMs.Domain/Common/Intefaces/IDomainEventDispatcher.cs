namespace Cpa.Fas.ProductMs.Domain.Common.Intefaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}