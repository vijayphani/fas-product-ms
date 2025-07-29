
using Cpa.Fas.ProductMs.Application;
using Cpa.Fas.ProductMs.Infrastructure;
using Cpa.Fas.ProductMs.WebApi.Middleware;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application and Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use custom correlationIdMiddleware
app.UseMiddleware<CorrelationIdMiddleware>();
// Use custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.UseHttpLogging();

app.Use(async (context, next) =>
{
    context.Response.Headers["MyResponseHeader"] =
        new string[] { "My Response Header Value" };

    await next();
});

app.MapControllers();

// Initialize the database (for demonstration purposes, run once)
// In a real application, use proper database migration tools.
//await InitializeDatabase(app.Services);

app.Run();


// Helper method to initialize the database schema
async Task InitializeDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        Console.WriteLine("Connection string 'DefaultConnection' is not configured. Skipping database initialization.");
        return;
    }

    try
    {
        // For SQL Server
        using var connection = new SqlConnection(connectionString);
        // For SQLite
        // using IDbConnection connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);

        await connection.OpenAsync();

        var createTableSql = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' and xtype='U')
            BEGIN
                CREATE TABLE Products (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Price DECIMAL(18, 2) NOT NULL,
                    Stock INT NOT NULL
                );
            END;";
        // For SQLite:
        // var createTableSql = @"
        //    CREATE TABLE IF NOT EXISTS Products (
        //        Id TEXT PRIMARY KEY,
        //        Name TEXT NOT NULL,
        //        Price REAL NOT NULL,
        //        Stock INTEGER NOT NULL
        //    );";

        using var command = connection.CreateCommand();
        command.CommandText = createTableSql;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("Database schema initialized successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
    }
}


namespace Cpa.Fas.ProductMs.WebApi
{
    public partial class Program;
}
