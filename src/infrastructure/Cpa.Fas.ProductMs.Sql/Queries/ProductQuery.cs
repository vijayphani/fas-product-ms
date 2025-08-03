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
    Stock,
    IsDeleted
FROM
    Products
WHERE
    IsDeleted = 0 AND
    Id = @Id
 ";

        #endregion GetProductById

        #region GetProductByName

        public const string GetProductByName = @"
SELECT
    Id,
    Name,
    Price,
    Stock,
    IsDeleted
FROM
    Products
WHERE
    Name = @Name
";

        #endregion GetProductByName
    }
}