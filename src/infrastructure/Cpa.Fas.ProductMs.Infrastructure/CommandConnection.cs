using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure;

public class CommandConnection : IDisposable
{
    public IDbConnection Connection { get; }

    public CommandConnection(IDbConnection connection)
    {
        Connection = connection;
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}