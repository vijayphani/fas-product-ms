using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Entities;
using Cpa.Fas.ProductMs.Infrastructure.Persistence.Repositories;
using Cpa.Fas.ProductMs.WebApi;
using System.Reflection;

namespace Cpa.Fas.ProductMs.Architecture.Tests
{
    public abstract class BaseTest
    {
        protected static readonly Assembly DomainAssembly = typeof(Product).Assembly;
        protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(CommandProductRepository).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
    }
}