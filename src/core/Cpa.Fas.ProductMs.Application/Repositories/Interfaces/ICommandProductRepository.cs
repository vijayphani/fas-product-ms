using Cpa.Fas.ProductMs.Domain.Entities;

namespace Cpa.Fas.ProductMs.Application.Repositories.Interfaces
{
    public interface ICommandProductRepository
    {
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);
    }
}