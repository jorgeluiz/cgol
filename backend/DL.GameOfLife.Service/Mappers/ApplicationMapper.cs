using AutoMapper;
using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Api.Mappers;

public static class ApplicationMapper
{
    public static void MainApplicationMap(this IMapperConfigurationExpression config)
    {
        config.MapItem<Board, BoardModel>();
        config.MapItem<BoardCell, BoardCellModel>();
    }

    public static void MapItem<TEntity, TModel>(this IMapperConfigurationExpression config, bool ignoreId = false)
    {
        if (ignoreId)
        {
            config.CreateMap<TEntity, TModel>().ForMember("Id", opt => opt.Ignore()).ForAllMembers(opts => opts.Condition((src, dest, member) => member != null));
            config.CreateMap<TModel, TEntity>().ForMember("Id", opt => opt.Ignore()).ForAllMembers(opts => opts.Condition((src, dest, member) => member != null));
        }
        else
        {
            config.CreateMap<TEntity, TModel>().ForAllMembers(opts => opts.Condition((src, dest, member) => member != null));
            config.CreateMap<TModel, TEntity>().ForAllMembers(opts => opts.Condition((src, dest, member) => member != null));
        }
    }

}
