using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DL.GameOfLife.Data.Context;
using DL.GameOfLife.Domain.Interfaces.Repositories;
using DL.GameOfLife.Data.Repositories;
using DL.GameOfLife.Domain.Options;

namespace DL.GameOfLife.Data.Registers;

public static class ServicesRegister
{
    public static void RegisterDataOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("MongoDbOptions"));
    }
    public static void RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDataOptions(configuration);
        services.AddSingleton<IGameOfLifeContext, GameOfLifeContext>();

        services.AddScoped<IBoardRepository, BoardRepository>();
    }
}
