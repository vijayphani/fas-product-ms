using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Events;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

namespace Cpa.Fas.ProductMs.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Create_ShouldCreateProductAndRaiseProductCreatedDomainEvent()
    {
        // Arrange
        var name = "Test Product";
        var price = 10.50m;
        var stock = 100;

        // Act
        var product = Product.Create(name, price, stock);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.Stock.Should().Be(stock);
        product.Id.Should().NotBeNull();
        product.Id.Value.Should().NotBeEmpty();

        product.DomainEvents.Should().ContainSingle();
        var domainEvent = product.DomainEvents.First() as ProductCreatedDomainEvent;
        domainEvent.Should().NotBeNull();
        domainEvent!.ProductId.Should().Be(product.Id);
        domainEvent.ProductName.Should().Be(name);
        domainEvent.Price.Should().Be(price);
        domainEvent.Stock.Should().Be(stock);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateNameAndPrice()
    {
        // Arrange
        var product = Product.Create("Old Name", 5.00m, 50);
        var newName = "New Name";
        var newPrice = 12.75m;

        // Act
        product.UpdateDetails(newName, newPrice);

        // Assert
        product.Name.Should().Be(newName);
        product.Price.Should().Be(newPrice);
    }

    [Theory]
    [InlineData("", 10.00, "Product name cannot be empty.")]
    [InlineData("Valid Name", 0, "Product price must be greater than zero.")]
    [InlineData("Valid Name", -5.00, "Product price must be greater than zero.")]
    public void UpdateDetails_ShouldThrowArgumentExceptionForInvalidInput(string name, decimal price, string expectedMessage)
    {
        // Arrange
        var product = Product.Create("Original", 10.00m, 10);

        // Act
        Action act = () => product.UpdateDetails(name, price);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expectedMessage + "*");
    }

    [Fact]
    public void IncreaseStock_ShouldIncreaseStock()
    {
        // Arrange
        var product = Product.Create("Test Product", 10.00m, 50);
        var quantity = 10;

        // Act
        product.IncreaseStock(quantity);

        // Assert
        product.Stock.Should().Be(60);
    }

    [Fact]
    public void IncreaseStock_ShouldThrowArgumentExceptionForNonPositiveQuantity()
    {
        // Arrange
        var product = Product.Create("Test Product", 10.00m, 50);

        // Act
        Action act = () => product.IncreaseStock(0);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity to increase must be greater than zero.*");
    }

    [Fact]
    public void DecreaseStock_ShouldDecreaseStock()
    {
        // Arrange
        var product = Product.Create("Test Product", 10.00m, 50);
        var quantity = 10;

        // Act
        product.DecreaseStock(quantity);

        // Assert
        product.Stock.Should().Be(40);
    }

    [Fact]
    public void DecreaseStock_ShouldThrowInvalidOperationExceptionForInsufficientStock()
    {
        // Arrange
        var product = Product.Create("Test Product", 10.00m, 50);
        var quantity = 60;

        // Act
        Action act = () => product.DecreaseStock(quantity);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Not enough stock available.");
    }

    [Fact]
    public void DecreaseStock_ShouldThrowArgumentExceptionForNonPositiveQuantity()
    {
        // Arrange
        var product = Product.Create("Test Product", 10.00m, 50);

        // Act
        Action act = () => product.DecreaseStock(0);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity to decrease must be greater than zero.*");
    }

    [Fact]
    public void ProductId_New_ShouldCreateNonEmptyGuid()
    {
        // Act
        var productId = ProductId.New();

        // Assert
        productId.Should().NotBeNull();
        productId.Value.Should().NotBeEmpty();
    }

    [Fact]
    public void ProductId_FromGuid_ShouldCreateWithGivenGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var productId = ProductId.FromGuid(guid);

        // Assert
        productId.Should().NotBeNull();
        productId.Value.Should().Be(guid);
    }

    [Fact]
    public void ProductId_FromGuid_ShouldThrowArgumentExceptionForEmptyGuid()
    {
        // Act
        Action act = () => ProductId.FromGuid(Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Product ID cannot be empty.*");
    }

    [Fact]
    public void ProductId_Equality_ShouldBeTrueForSameValue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id1 = ProductId.FromGuid(guid);
        var id2 = ProductId.FromGuid(guid);

        // Assert
        id1.Should().Be(id2);
        (id1 == id2).Should().BeTrue();
    }

    [Fact]
    public void ProductId_Equality_ShouldBeFalseForDifferentValue()
    {
        // Arrange
        var id1 = ProductId.New();
        var id2 = ProductId.New();

        // Assert
        id1.Should().NotBe(id2);
        (id1 != id2).Should().BeTrue();
    }

    [Fact]
    public void ProductId_ImplicitConversionToGuid_ShouldWork()
    {
        // Arrange
        var guid = Guid.NewGuid();
        ProductId productId = guid;

        // Act
        Guid convertedGuid = productId;

        // Assert
        convertedGuid.Should().Be(guid);
    }

    [Fact]
    public void ProductId_ImplicitConversionFromGuid_ShouldWork()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        ProductId productId = guid;

        // Assert
        productId.Value.Should().Be(guid);
    }

    [Fact]
    public void ProductId_ImplicitConversionToString_ShouldWork()
    {
        // Arrange
        var guid = Guid.NewGuid();
        ProductId productId = guid;

        // Act
        string convertedString = productId;

        // Assert
        convertedString.Should().Be(guid.ToString());
    }

    [Fact]
    public void ProductId_ImplicitConversionFromString_ShouldWork()
    {
        // Arrange
        var guidString = Guid.NewGuid().ToString();

        // Act
        ProductId productId = guidString;

        // Assert
        productId.Value.Should().Be(Guid.Parse(guidString));
    }
}
