using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Domain.Repositories;
using Cpa.Fas.ProductMs.Domain.ValueObjects;
using Cpa.Fas.ProductMs.Sql.Commands;
using Dapper;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories
{
    public class CommandProductRepository : ICommandProductRepository
    {
        private readonly IDbConnection _commandConnection;
        private readonly IDbTransaction _transaction;

        public CommandProductRepository(
            CommandConnection commandConnection,
            IDbTransaction transaction)
        {
            _commandConnection = commandConnection.Connection;
            _transaction = transaction;
        }

        public CommandProductRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _commandConnection = connection;
            _transaction = transaction;
        }

        public async Task AddAsync(Product product)
        {
            var sql = ProductCommand.AddProduct;
            await _commandConnection.ExecuteAsync(sql, new
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
            await _commandConnection.ExecuteAsync(sql, new
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
            await _commandConnection.ExecuteAsync(sql, new { Id = id.Value }, _transaction);
        }
    }
}