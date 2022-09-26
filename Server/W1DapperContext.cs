using Microsoft.Data.SqlClient;
using System.Data;

namespace W1.Server;

public class W1DapperContext
{
    private readonly string connStr; 
    public W1DapperContext(IConfiguration configuration)
    {
        connStr = configuration.GetConnectionString("W1");
    }

    public IDbConnection CreateConnection() => new SqlConnection(connStr);
}
