using System.Diagnostics.CodeAnalysis;

namespace Cpa.Fas.ProductMs.Sql.Commands
{
    [ExcludeFromCodeCoverage]
    public static class ProductCommand
    {
        #region insert
        public const string AddProduct = @"
INSERT INTO Products (
    Id, 
    Name, 
    Price, 
    Stock,
    CreatedBy,
    CreatedAt,
    UpdatedBy,
    UpdatedAt
) VALUES (
    @Id, 
    @Name, 
    @Price, 
    @Stock,
    @CreatedBy,
    @CreatedAt, 
    @UpdatedBy, 
    @UpdatedAt
)
";
        #endregion

        #region Update
        public const string UpdateProduct = @"
UPDATE Products 
SET 
    Name = @Name, 
    Price = @Price, 
    Stock = @Stock, 
    UpdatedBy = @UpdatedBy,
    UpdatedAt = @UpdatedAt
WHERE 
    Id = @Id
";
        #endregion

        #region Delete  
        public const string DeleteProduct = @"
DELETE 
FROM 
    Products 
WHERE 
    Id = @Id";
        #endregion

    }
}
