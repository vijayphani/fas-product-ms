using Cpa.Fas.ProductMs.Application.Repositories.Interfaces;
using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.DapperModels;
using Cpa.Fas.ProductMs.Sql.Queries;
using Dapper;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories
{
    public class QueryProductRepository : IQueryProductRepository
    {
        private readonly IDbConnection _queryConnection;

        public QueryProductRepository(IDbConnection connection)
        {
            _queryConnection = connection;
        }

        public async Task<Product?> GetByIdAsync(ProductId id)
        {
            var sql = ProductQuery.GetProductById;
            var productModel = await _queryConnection.QuerySingleOrDefaultAsync<ProductModel>(sql, new { Id = id.Value });

            if (productModel == null)
            {
                return null;
            }

            return new Product(ProductId.FromGuid(productModel.Id), productModel.Name, productModel.Price, productModel.Stock, productModel.IsDeleted);
        }
    }
}