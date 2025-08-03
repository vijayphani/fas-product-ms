using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Domain.Repositories
{
    public interface IQueryProductRepository
    {
        Task<Product?> GetByIdAsync(ProductId id);
    }
}