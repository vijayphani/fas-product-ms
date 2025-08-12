namespace Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommandRequest(string Name, decimal Price, int Stock);
}