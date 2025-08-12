using Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById;
using Cpa.Fas.ProductMs.Application.Repositories.Interfaces;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Cpa.Fas.ProductMs.Application.Tests.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IQueryProductRepository> _mockQueryProductRepository;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockQueryProductRepository = new Mock<IQueryProductRepository>();
            _handler = new GetProductByIdQueryHandler(_mockQueryProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto_WhenProductExists()
        {
            // Arrange
            var productId = ProductId.New();
            var productName = "Existing Product";
            var productPrice = 25.00m;
            var productStock = 10;

            var product = new GetProductByIdQueryResponse(productId, productName, productPrice, productStock, false);

            _mockQueryProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            var query = new GetProductByIdQuery(productId.Value);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId.Value);
            result.Name.Should().Be(productName);
            result.Price.Should().Be(productPrice);
            result.Stock.Should().Be(productStock);

            _mockQueryProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = ProductId.New();
            _mockQueryProductRepository.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((GetProductByIdQueryResponse?)null); // Simulate product not found

            var query = new GetProductByIdQuery(productId.Value);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mockQueryProductRepository.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }
    }
}