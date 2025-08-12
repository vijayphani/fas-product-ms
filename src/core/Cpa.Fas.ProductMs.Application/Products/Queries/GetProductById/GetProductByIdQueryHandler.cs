using Cpa.Fas.ProductMs.Application.Repositories.Interfaces;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using MediatR;

namespace Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse?>
    {
        private readonly IQueryProductRepository _queryProductRepository;

        public GetProductByIdQueryHandler(IQueryProductRepository queryProductRepository)
        {
            _queryProductRepository = queryProductRepository;
        }

        public async Task<GetProductByIdQueryResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productId = ProductId.FromGuid(request.Id);
            var product = await _queryProductRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return null;
            }
            return new GetProductByIdQueryResponse(product.Id, product.Name, product.Price, product.Stock, product.IsDeleted);
        }
    }
}