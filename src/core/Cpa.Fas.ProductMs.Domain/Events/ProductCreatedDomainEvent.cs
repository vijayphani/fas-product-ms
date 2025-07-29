using Cpa.Fas.ProductMs.Domain.Common;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using MediatR;

namespace Cpa.Fas.ProductMs.Domain.Events
{
    public class ProductCreatedDomainEvent : BaseDomainEvent, INotification
    {
        public ProductCreatedDomainEvent(ProductId productId, string productName, decimal price, int stock)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Stock = stock;
        }

        public ProductId ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }
        public int Stock { get; }
    }
}