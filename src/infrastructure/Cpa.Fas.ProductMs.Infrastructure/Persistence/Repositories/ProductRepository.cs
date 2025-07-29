using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Repositories;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.DapperModels;
using Cpa.Fas.ProductMs.Sql.Commands;
using Cpa.Fas.ProductMs.Sql.Queries;
using Dapper;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        // The UnitOfWork instance is implicitly passed via the constructor
        // to ensure all operations use the same connection and transaction.
        public ProductRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<Product?> GetByIdAsync(ProductId id)
        {
            var sql = ProductQuery.GetProductById;
            var productModel = await _connection.QuerySingleOrDefaultAsync<ProductModel>(sql, new { Id = id.Value }, _transaction);

            if (productModel == null)
            {
                return null;
            }

            // Reconstruct domain entity from Dapper model
            return new Product(ProductId.FromGuid(productModel.Id), productModel.Name, productModel.Price, productModel.Stock, productModel.IsDeleted);
        }

        public async Task AddAsync(Product product)
        {
            var sql = ProductCommand.AddProduct;
            await _connection.ExecuteAsync(sql, new
            {
                Id = product.Id.Value,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedBy,
                product.CreatedAt,
                product.UpdatedBy,
                product.UpdatedAt
            }, _transaction);
        }

        public async Task UpdateAsync(Product product)
        {
            var sql = ProductCommand.UpdateProduct;
            await _connection.ExecuteAsync(sql, new
            {
                product.Name,
                product.Price,
                product.Stock,
                product.UpdatedBy,
                product.UpdatedAt,
                Id = product.Id.Value
            }, _transaction);
        }

        public async Task DeleteAsync(ProductId id)
        {
            var sql = ProductCommand.DeleteProduct;
            await _connection.ExecuteAsync(sql, new { Id = id.Value }, _transaction);
        }
    }
}