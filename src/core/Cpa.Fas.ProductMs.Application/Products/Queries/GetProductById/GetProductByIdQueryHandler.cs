using Cpa.Fas.ProductMs.Domain.Repositories;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using MediatR;

namespace Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productId = ProductId.FromGuid(request.Id);
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return null;
            }

            return new ProductDto(product.Id.Value, product.Name, product.Price, product.Stock);
        }
    }

}
