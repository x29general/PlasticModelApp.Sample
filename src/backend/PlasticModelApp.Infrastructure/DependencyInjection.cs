using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PlasticModelApp.Application.Masters.Interfaces;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Tags.Interfaces;
using PlasticModelApp.Infrastructure.Data;
using PlasticModelApp.Infrastructure.Queries;

namespace PlasticModelApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "PostgreSQL";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? configuration["ConnectionStrings:DefaultConnection"]
            ?? string.Empty;
        var dbSchema = configuration["DbSchema"] ?? "public";
        var migrationsHistorySchema = configuration["MigrationsHistorySchema"] ?? dbSchema;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:DefaultConnection が設定されていません。");
        }

        if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) || provider.Equals("Npgsql", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, npgsql =>
                {
                    // Keep migrations history table in the configured schema to match existing DB state
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", migrationsHistorySchema);
                }));
        }
        else
        {
            throw new NotSupportedException($"未対応の DatabaseProvider: {provider}");
        }

        services.AddScoped<IPaintQueryService, PaintQueryService>();
        services.AddScoped<IMasterQueryService, MasterQueryService>();
        services.AddScoped<ITagQueryService, TagQueryService>();

        return services;
    }
}
