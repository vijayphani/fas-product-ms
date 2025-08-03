using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Common;
using Cpa.Fas.ProductMs.Domain.Common.Intefaces;
using Cpa.Fas.ProductMs.Domain.Repositories;
using Cpa.Fas.ProductMs.Infrastructure.Persistence;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Command and Query connections
        services.AddScoped<IDbConnection>(sp =>
        {
            var connectionString = configuration.GetConnectionString("CommandConnection")
                ?? throw new InvalidOperationException("CommandConnection string is not configured.");
            return new SqlConnection(connectionString);
        });

        // Query connection string
        services.AddScoped<IDbConnection>(sp =>
        {
            var connectionString = configuration.GetConnectionString("QueryConnection")
                ?? throw new InvalidOperationException("QueryConnection string is not configured.");
            return new SqlConnection(connectionString);
        });

        // Register named connections for CQRS
        services.AddScoped<CommandConnection>();
        services.AddScoped<QueryConnection>();

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
        services.AddScoped<ICommandProductRepository, CommandProductRepository>();
        services.AddScoped<IQueryProductRepository, QueryProductRepository>();

        // Register Domain Event Dispatcher
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}