using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure;

public class QueryConnection : IDisposable
{
    public IDbConnection Connection { get; }

    public QueryConnection(IDbConnection connection)
    {
        Connection = connection;
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}