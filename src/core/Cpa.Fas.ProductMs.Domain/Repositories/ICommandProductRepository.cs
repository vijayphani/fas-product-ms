using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Domain.Repositories
{
    public interface ICommandProductRepository
    {
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}