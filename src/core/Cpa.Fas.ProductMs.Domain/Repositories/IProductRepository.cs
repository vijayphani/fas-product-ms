using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(ProductId id);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(ProductId id);
    }
}