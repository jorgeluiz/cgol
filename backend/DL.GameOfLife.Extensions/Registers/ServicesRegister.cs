using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Domain.Options;
using DL.GameOfLife.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DL.GameOfLife.Extensions.Registers;

public static class ServicesRegister
{
    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GameOfLifeOptions>(configuration.GetSection("GameOfLifeOptions"));
    }
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions(configuration);

        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IGameOfLifeService, GameOfLifeService>();
    }
}
