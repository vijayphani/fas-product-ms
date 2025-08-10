using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Application.Repositories.Interfaces;
using Cpa.Fas.ProductMs.Domain.Entities;
using MediatR;

namespace Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly ICommandProductRepository _commandProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(ICommandProductRepository commandProductRepository, IUnitOfWork unitOfWork)
        {
            _commandProductRepository = commandProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name, request.Price, request.Stock, request.userGuid);

            await _commandProductRepository.AddAsync(product);
            _unitOfWork.AddEntityWithEvents(product);
            await _unitOfWork.CommitAsync(cancellationToken); // This will also dispatch domain events

            return product.Id.Value;
        }
    }
}