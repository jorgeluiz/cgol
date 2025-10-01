using System;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Service;
using Microsoft.Extensions.DependencyInjection;

namespace DL.GameOfLife.Extensions.Registers;

public static class ServiceRegister
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IGameOfLifeService, GameOfLifeService>();
    }
}
