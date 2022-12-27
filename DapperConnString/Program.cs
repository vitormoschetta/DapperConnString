using System.Data;
using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var connection = GetConnection(Provider.SqlServer);
connection.Open();
var sql = "select * from Product";
var items = connection.Query(sql);

foreach (var item in items)
{
    Console.WriteLine(JsonSerializer.Serialize(item));
}


IDbConnection GetConnection(Provider provider)
{
    var configuration = Host.CreateDefaultBuilder().Build().Services.GetService<IConfiguration>() ?? throw new Exception("No configuration");

    var connectionString = string.Empty;

    switch (provider)
    {
        case Provider.SqlServer:
            connectionString = configuration.GetConnectionString("SqlServer") ?? throw new Exception("No connection string");
            break;
        default:
            throw new Exception("Provider not supported");
    }

    return new SqlConnection(connectionString);
}

enum Provider
{
    SqlServer,
    MySql,
    PostgreSql,
    Oracle,
    Sqlite
}