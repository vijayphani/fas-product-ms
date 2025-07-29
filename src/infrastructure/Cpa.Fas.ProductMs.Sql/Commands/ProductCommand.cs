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
        // We do only soft delete in this case, setting IsDeleted to 1

        public const string DeleteProduct = @"
UPDATE Products 
SET 
    IsDeleted = 1 
WHERE 
    Id = @Id
";
        #endregion

    }
}
