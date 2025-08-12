namespace Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById
{
    public record GetProductByIdQueryResponse(Guid Id, string Name, decimal Price, int Stock, bool IsDeleted);
}