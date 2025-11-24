using JsMate.Application.Interfaces;
using JsMate.Application.Services;
using JsMate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsMate.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Database:ConnectionString"] ?? "ChessData.db";

        services.AddSingleton<IBoardRepository>(sp =>
        {
            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LiteDbBoardRepository>>();
            return new LiteDbBoardRepository(connectionString, logger);
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IChessService, ChessService>();
        return services;
    }
}
