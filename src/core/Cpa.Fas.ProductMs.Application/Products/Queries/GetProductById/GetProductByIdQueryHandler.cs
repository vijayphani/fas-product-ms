using Cpa.Fas.ProductMs.Application.Repositories.Interfaces;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using MediatR;

namespace Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IQueryProductRepository _queryProductRepository;

        public GetProductByIdQueryHandler(IQueryProductRepository queryProductRepository)
        {
            _queryProductRepository = queryProductRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productId = ProductId.FromGuid(request.Id);
            var product = await _queryProductRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return null;
            }
            return new ProductDto(product.Id.Value, product.Name, product.Price, product.Stock);
        }
    }
}