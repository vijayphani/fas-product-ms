using Cpa.Fas.ProductMs.Domain.Common;

namespace Cpa.Fas.ProductMs.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}
