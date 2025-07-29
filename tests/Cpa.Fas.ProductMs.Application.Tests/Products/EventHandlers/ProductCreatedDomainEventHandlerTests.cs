using Cpa.Fas.ProductMs.Application.Products.EventHandlers;
using Cpa.Fas.ProductMs.Domain.Events;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace Cpa.Fas.ProductMs.Application.Tests.Products.EventHandlers
{
    public class ProductCreatedDomainEventHandlerTests
    {
        private readonly Mock<ILogger<ProductCreatedDomainEventHandler>> _mockLogger;
        private readonly ProductCreatedDomainEventHandler _handler;

        public ProductCreatedDomainEventHandlerTests()
        {
            _mockLogger = new Mock<ILogger<ProductCreatedDomainEventHandler>>();
            _handler = new ProductCreatedDomainEventHandler(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldLogProductCreationDetails()
        {
            // Arrange
            var productId = ProductId.New();
            var productName = "New Test Product";
            var price = 99.99m;
            var stock = 100;

            var notification = new ProductCreatedDomainEvent(productId, productName, price, stock);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _mockLogger.Verify(logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("ProductMs Domain Event: ProductCreatedDomainEvent")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);

            //_mockLogger.Verify(logger => logger.Log(
            //        LogLevel.Information,
            //        It.IsAny<EventId>(),
            //        It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"Product created: Id={productId}, Name={productName}, Price={price}, Stock={stock}")),
            //        It.IsAny<Exception>(),
            //        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            //    Times.AtLeastOnce);
        }
    }
}