namespace Cpa.Fas.ProductMs.Infrastructure.Persistence.DapperModels
{
    // This model is used by Dapper to map database rows to C# objects.
    // It's a simple representation of the table schema.
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; } = false; // Default to false, indicating the product is not deleted
    }
}