using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Cpa.Fas.ProductMs.Infrastructure;

public class QueryConnection : IDisposable
{
    public IDbConnection Connection { get; }

    public QueryConnection(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("QueryConnection")
            ?? throw new InvalidOperationException("QueryConnection string is not configured.");
        Connection = new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}
