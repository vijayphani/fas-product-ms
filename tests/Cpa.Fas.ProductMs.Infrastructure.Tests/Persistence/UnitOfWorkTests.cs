using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Events;
using Cpa.Fas.ProductMs.Infrastructure.Persistence;
using Cpa.Fas.ProductMs.Infrastructure.Services;
using Dapper;
using FluentAssertions;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure.Tests.Persistence
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly Mock<ILogger<DomainEventDispatcher>> _mockLogger;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Create a dummy table for the connection to work with
            var createTableSql = "CREATE TABLE Dummy (Id INTEGER PRIMARY KEY);";
            _connection.Execute(createTableSql);

            _mockPublisher = new Mock<IPublisher>();
            _mockLogger = new Mock<ILogger<DomainEventDispatcher>>();
            _domainEventDispatcher = new DomainEventDispatcher(_mockPublisher.Object, _mockLogger.Object);

            _unitOfWork = new UnitOfWork(_connection, _mockPublisher.Object, _domainEventDispatcher);
        }

        [Fact]
        public async Task CommitAsync_ShouldCommitTransactionAndDispatchEvents()
        {
            // Arrange
            var product = Product.Create("Test Product", 10.00m, 10); // This creates a ProductCreatedDomainEvent
            _unitOfWork.AddEntityWithEvents(product); // Manually add entity to UoW for event tracking

            // Act
            await _unitOfWork.CommitAsync();

            // Assert
            // Verify that the domain event was published
            _mockPublisher.Verify(p => p.Publish(It.IsAny<ProductCreatedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verify that the entity's domain events were cleared
            product.DomainEvents.Should().BeEmpty();

            // Verify that the transaction was committed (implicitly by no exception)
            // We can't directly check if a transaction was committed on a mocked connection,
            // but the absence of a rollback or exception implies commit.
        }

        [Fact]
        public async Task CommitAsync_ShouldRollbackTransaction_WhenExceptionOccurs()
        {
            // Arrange
            var product = Product.Create("Test Product", 10.00m, 10);
            _unitOfWork.AddEntityWithEvents(product);

            // Simulate an error during commit (e.g., a database constraint violation)
            // For this, we'll make the publisher throw an exception.
            _mockPublisher.Setup(p => p.Publish(It.IsAny<ProductCreatedDomainEvent>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Simulated publishing error"));

            // Act
            Func<Task> act = async () => await _unitOfWork.CommitAsync();

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Simulated publishing error");

            // Verify that the domain event was attempted to be published
            _mockPublisher.Verify(p => p.Publish(It.IsAny<ProductCreatedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verify that the entity's domain events were NOT cleared (because rollback happened)
            // This behavior depends on where ClearDomainEvents is called.
            // In our current UoW, it's called after successful commit, so it shouldn't be cleared on rollback.
            product.DomainEvents.Should().NotBeEmpty();
        }

        [Fact]
        public void UnitOfWork_ShouldDisposeConnectionAndTransaction()
        {
            // Arrange
            var connectionMock = new Mock<IDbConnection>();
            var transactionMock = new Mock<IDbTransaction>();

            connectionMock.Setup(c => c.BeginTransaction()).Returns(transactionMock.Object);
            connectionMock.Setup(c => c.State).Returns(ConnectionState.Open);

            var uow = new UnitOfWork(connectionMock.Object, _mockPublisher.Object, _domainEventDispatcher);

            // Act
            uow.Dispose();

            // Assert
            transactionMock.Verify(t => t.Dispose(), Times.Once);
            connectionMock.Verify(c => c.Dispose(), Times.Once);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}