using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Application.Repositories.Interfaces
{
    public interface IQueryProductRepository
    {
        Task<Product?> GetByIdAsync(ProductId id);
    }
}