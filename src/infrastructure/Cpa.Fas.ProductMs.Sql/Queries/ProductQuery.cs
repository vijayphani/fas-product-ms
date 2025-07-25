using System.Diagnostics.CodeAnalysis;

namespace Cpa.Fas.ProductMs.Sql.Queries
{
    [ExcludeFromCodeCoverage]
    public static class ProductQuery
    {
        #region GetProductById
        public const string GetProductById = @"
SELECT 
    Id, 
    Name, 
    Price, 
    Stock 
FROM 
    Products 
WHERE 
    Id = @Id
 ";
        #endregion

        #region GetProductByName
        public const string GetProductByName = @"
SELECT 
    Id, 
    Name, 
    Price, 
    Stock 
FROM 
    Products 
WHERE 
    Name = @Name
";
        #endregion


    }
}
