using Cpa.Fas.ProductMs.Application.Common.Interfaces;

namespace Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, decimal Price, int Stock) : ICommand<Guid>;
}
