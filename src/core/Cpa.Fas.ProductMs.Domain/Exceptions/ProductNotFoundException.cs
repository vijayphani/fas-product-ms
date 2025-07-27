using Cpa.Fas.ProductMs.Domain.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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