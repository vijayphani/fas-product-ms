using Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Application.Repositories.Interfaces
{
    public interface IQueryProductRepository
    {
        Task<GetProductByIdQueryResponse?> GetByIdAsync(ProductId id);
    }
}