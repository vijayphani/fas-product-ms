using Cpa.Fas.ProductMs.Domain.Common;
using Cpa.Fas.ProductMs.Domain.Events;
using Cpa.Fas.ProductMs.Domain.ValueObjects;

namespace Cpa.Fas.ProductMs.Domain.Entities
{
    public class Product : BaseEntity, IAggregateRoot
    {
        // Private constructor for EF Core or Dapper deserialization
        // and to enforce creation through a factory method or static method.
        private Product() { }

        // Public constructor for creating new instances within the domain.
        // This can be simplified or made private if a static factory method is preferred.
        public Product(ProductId id, string name, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public ProductId Id { get; private set; } = ProductId.New(); // Initialize with a new ID
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        // Static factory method to create a new Product and raise a domain event.
        public static Product Create(string name, decimal price, int stock)
        {
            var product = new Product(ProductId.New(), name, price, stock);
            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.Price, product.Stock));
            return product;
        }

        public void UpdateDetails(string name, decimal price)
        {
            // Add validation or business rules here
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }
            if (price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.", nameof(price));
            }

            Name = name;
            Price = price;
            // Optionally, raise a ProductUpdatedDomainEvent here
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity to increase must be greater than zero.", nameof(quantity));
            }
            Stock += quantity;
            // Optionally, raise a StockIncreasedDomainEvent here
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity to decrease must be greater than zero.", nameof(quantity));
            }
            if (Stock < quantity)
            {
                throw new InvalidOperationException("Not enough stock available.");
            }
            Stock -= quantity;
            // Optionally, raise a StockDecreasedDomainEvent here
        }
    }
}
