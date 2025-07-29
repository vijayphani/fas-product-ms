using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using System.Data;
using System.Data.SQLite;

namespace Cpa.Fas.ProductMs.Infrastructure.Tests.Persistence.Repositories;

public class ProductRepositoryTests : IDisposable
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;
    private readonly ProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
        _connection.Open();

        // Begin a transaction for each test to ensure isolation and rollback
        _transaction = _connection.BeginTransaction();

        // Create the table schema
        var createTableSql = @"
CREATE TABLE [Products](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL
);";

        _connection.Execute(createTableSql, transaction: _transaction);

        _productRepository = new ProductRepository(_connection, _transaction);
    }

    [Fact]
    public async Task AddAsync_ShouldAddProductToDatabase()
    {
        var userGuid = Guid.NewGuid();
        // Arrange
        var product = Product.Create("Test Product", 15.99m, 200, userGuid);

        // Act
        await _productRepository.AddAsync(product);

        // Assert
        var retrievedProduct = await _productRepository.GetByIdAsync(product.Id);
        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Id.Should().Be(product.Id);
        retrievedProduct.Name.Should().Be(product.Name);
        retrievedProduct.Price.Should().Be(product.Price);
        retrievedProduct.Stock.Should().Be(product.Stock);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = ProductId.New();
        var product = new Product(productId, "Existing Product", 100.00m, 50);
        await _productRepository.AddAsync(product); // Add product directly for setup

        // Act
        var retrievedProduct = await _productRepository.GetByIdAsync(productId);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Id.Should().Be(productId);
        retrievedProduct.Name.Should().Be("Existing Product");
        retrievedProduct.Price.Should().Be(100.00m);
        retrievedProduct.Stock.Should().Be(50);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var nonExistentId = ProductId.New();

        // Act
        var retrievedProduct = await _productRepository.GetByIdAsync(nonExistentId);

        // Assert
        retrievedProduct.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductInDatabase()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var product = Product.Create("Original Product", 20.00m, 100, userGuid);
        await _productRepository.AddAsync(product);

        product.UpdateDetails("Updated Product", 25.50m);
        product.IncreaseStock(10); // Also test stock update

        // Act
        await _productRepository.UpdateAsync(product);

        // Assert
        var updatedProduct = await _productRepository.GetByIdAsync(product.Id);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be("Updated Product");
        updatedProduct.Price.Should().Be(25.50m);
        updatedProduct.Stock.Should().Be(110);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotRemoveProductFromDatabase()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var product = Product.Create("Product To Delete", 5.00m, 10, userGuid);
        await _productRepository.AddAsync(product);

        // Act
        await _productRepository.DeleteAsync(product.Id);

        // Assert
        var deletedProduct = await _productRepository.GetByIdAsync(product.Id);
        deletedProduct.Should().NotBeNull();
        deletedProduct.IsDeleted.Should().Be(true);
    }

    public void Dispose()
    {
        // Rollback the transaction to ensure tests are isolated and don't affect each other
        _transaction.Rollback();
        _transaction.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}

// Helper for Dapper Execute for SQLite
internal static class DapperSqliteExtensions
{
    public static int Execute(this IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        return Dapper.SqlMapper.Execute(cnn, sql, param, transaction, commandTimeout, commandType);
    }
}