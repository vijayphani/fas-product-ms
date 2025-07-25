using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Repositories;
using Cpa.Fas.ProductMs.Infrastructure.Persistence;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories;
using Cpa.Fas.ProductMs.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Cpa.Fas.ProductMs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register IDbConnection
        services.AddScoped<IDbConnection>(sp =>
        {
            // Use SqlConnection for SQL Server. For SQLite, use SqliteConnection.
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection connection string is not configured.");
            }
            return new SqlConnection(connectionString);
            // return new SqliteConnection(connectionString); // For SQLite
        });

        // Register UnitOfWork as scoped
        services.AddScoped<UnitOfWork>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());

        // Register IDbTransaction (scoped to UnitOfWork's connection)
        services.AddScoped<IDbTransaction>(sp =>
        {
            var unitOfWork = sp.GetRequiredService<UnitOfWork>();
            return unitOfWork.Transaction;
        });

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        // Register Domain Event Dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}