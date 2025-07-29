using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Repositories;
using MediatR;

namespace Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name, request.Price, request.Stock, request.userGuid);

            await _productRepository.AddAsync(product);
            _unitOfWork.AddEntityWithEvents(product);
            await _unitOfWork.CommitAsync(cancellationToken); // This will also dispatch domain events

            return product.Id.Value;
        }
    }
}