using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlasticModelApp.Application.Masters.Interfaces;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Tags.Interfaces;
using PlasticModelApp.Infrastructure.Data;
using PlasticModelApp.Infrastructure.Queries;

namespace PlasticModelApp.Infrastructure.UnitTests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_Should_Throw_When_ConnectionString_Is_Missing()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var act = () => services.AddInfrastructure(configuration);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddInfrastructure_Should_Throw_For_Unsupported_Provider()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = "SqlServer",
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test;Username=u;Password=p"
            })
            .Build();

        var act = () => services.AddInfrastructure(configuration);

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void AddInfrastructure_Should_Register_DbContext_And_QueryServices_For_PostgreSql_Default()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test;Username=u;Password=p"
            })
            .Build();

        services.AddInfrastructure(configuration);
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IPaintQueryService>().Should().BeOfType<PaintQueryService>();
        scope.ServiceProvider.GetRequiredService<IMasterQueryService>().Should().BeOfType<MasterQueryService>();
        scope.ServiceProvider.GetRequiredService<ITagQueryService>().Should().BeOfType<TagQueryService>();
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.ProviderName
            .Should().Be("Npgsql.EntityFrameworkCore.PostgreSQL");
    }

    [Fact]
    public void AddInfrastructure_Should_Register_For_Npgsql_Alias()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = "Npgsql",
                ["MigrationsHistorySchema"] = "custom_history_schema",
                ["DbSchema"] = "custom_schema",
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test2;Username=u;Password=p",
            })
            .Build();

        services.AddInfrastructure(configuration);
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.ProviderName.Should().Be("Npgsql.EntityFrameworkCore.PostgreSQL");
    }
}
