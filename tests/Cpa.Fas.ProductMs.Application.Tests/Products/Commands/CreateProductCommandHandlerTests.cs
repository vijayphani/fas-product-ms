using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct;
using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Cpa.Fas.ProductMs.Application.Tests.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<ICommandProductRepository> _mockCommandProductRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockCommandProductRepository = new Mock<ICommandProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new CreateProductCommandHandler(_mockCommandProductRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddProductAndCommitUnitOfWork()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var command = new CreateProductCommand("Test Product", 10.99m, 50, userGuid);

            _mockUnitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); // Simulate successful commit

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty(); // Expect a new product ID (Guid)

            _mockCommandProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}