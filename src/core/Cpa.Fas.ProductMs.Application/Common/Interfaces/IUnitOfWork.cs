using Cpa.Fas.ProductMs.Domain.Common;

namespace Cpa.Fas.ProductMs.Application.Common.Interfaces;

public interface IUnitOfWork
{
    void AddEntityWithEvents(BaseEntity entity);

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}