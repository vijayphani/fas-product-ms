using Cpa.Fas.ProductMs.Domain.Common;

namespace Cpa.Fas.ProductMs.Domain.ValueObjects
{
    public class ProductId : ValueObject
    {
        public Guid Value { get; }

        private ProductId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(value));
            }
            Value = value;
        }

        public static ProductId New() => new(Guid.NewGuid());
        public static ProductId FromGuid(Guid value) => new(value);
        public static ProductId FromString(string value) => new(Guid.Parse(value));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        // Implicit conversion from Guid to ProductId
        public static implicit operator ProductId(Guid value) => new(value);

        // Implicit conversion from ProductId to Guid
        public static implicit operator Guid(ProductId productId) => productId.Value;

        // Implicit conversion from string to ProductId
        public static implicit operator ProductId(string value) => new(Guid.Parse(value));

        // Implicit conversion from ProductId to string
        public static implicit operator string(ProductId productId) => productId.Value.ToString();
    }

}
