using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Infrastructure.UnitTests.Fixtures;

public sealed class SqliteDbContextFactory : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteDbContextFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        return new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
