using Cpa.Fas.ProductMs.Domain.Exceptions.Base;

namespace Cpa.Fas.ProductMs.Domain.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid productId)
            : base($"The product with the identifier {productId} was not found.")
        {
        }
    }
}