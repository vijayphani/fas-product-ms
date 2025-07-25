namespace Cpa.Fas.ProductMs.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}