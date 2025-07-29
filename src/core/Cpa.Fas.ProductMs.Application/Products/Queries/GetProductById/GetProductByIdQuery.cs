using Cpa.Fas.ProductMs.Application.Common.Interfaces;

namespace Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto?>;
}