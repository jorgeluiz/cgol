using AutoMapper;
using DL.GameOfLife.Api.Mappers;
using DL.GameOfLife.Extensions.Registers;
using DL.GameOfLife.Data.Registers;
using DL.GameOfLife.Api.ErrorHandling;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
    {
        config.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "GameOfLife API",
            Version = "v1"
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

#region Mapper
builder.Services.AddSingleton(
    new MapperConfiguration(config =>
    {
        config.MainApplicationMap();
    }).CreateMapper()
);
#endregion

#region Registers

builder.Services.AddTransient<ErrorHandlingMiddleware>();

builder.Services.RegisterServices(builder.Configuration);
builder.Services.RegisterDataServices(builder.Configuration);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
