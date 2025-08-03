using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;

namespace Cpa.Fas.ProductMs.Infrastructure;

public class CommandConnection : IDisposable
{
    public IDbConnection Connection { get; }
    

    public CommandConnection(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CommandConnection")
            ?? throw new InvalidOperationException("CommandConnection string is not configured.");
        Connection = new SqlConnection(connectionString);
    }

    public CommandConnection(SQLiteConnection connection)
    {
        this.Connection = connection;
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}
