using Cpa.Fas.ProductMs.Application.Common.Interfaces;
using Cpa.Fas.ProductMs.Domain.Common;
using Cpa.Fas.ProductMs.Domain.Common.Intefaces;
using MediatR;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private readonly IPublisher _publisher;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        // A list to keep track of entities that have domain events
        private readonly List<BaseEntity> _entitiesWithEvents = new();

        public UnitOfWork(CommandConnection commandConnection, IPublisher publisher, IDomainEventDispatcher domainEventDispatcher)
        {
            _connection = commandConnection.Connection;
            _publisher = publisher;
            _domainEventDispatcher = domainEventDispatcher;

            // Open connection if not already open
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("Transaction not started.");

        // This method should be called by repositories when an entity with events is added/updated.
        public void AddEntityWithEvents(BaseEntity entity)
        {
            if (!_entitiesWithEvents.Contains(entity))
            {
                _entitiesWithEvents.Add(entity);
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Execute any pending database operations (not explicit with Dapper, but conceptually here)
                // For Dapper, changes are usually committed immediately per command,
                // but the transaction ensures atomicity across multiple commands.

                _transaction?.Commit();

                // Dispatch domain events AFTER successful commit
                await _domainEventDispatcher.DispatchAndClearEvents(_entitiesWithEvents);

                // Clear entities with events after dispatch
                _entitiesWithEvents.Clear();

                return 1; // Indicate success, or number of affected rows if tracked.
            }
            catch (Exception)
            {
                _transaction?.Rollback();
                throw; // Re-throw the exception to propagate it up the call stack
            }
            finally
            {
                // Dispose the transaction after commit or rollback
                _transaction?.Dispose();
                _transaction = null; // Set to null to prevent re-use
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection.Dispose();
            }
        }
    }
}